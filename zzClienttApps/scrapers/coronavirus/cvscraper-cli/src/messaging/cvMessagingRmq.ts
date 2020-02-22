import amqp from "amqplib/callback_api"; 
import { connect, ConsumeMessage } from 'amqplib';

export class cvMessagingRmq {
    private amqpConn: amqp.Connection;
    private connStr = 'amqp://noekmbda:jtKtg3LU2uEQOCJRpEgMhdSqf2N7S_Cv@gull.rmq.cloudamqp.com/noekmbda';
    private pubChannel = null;
    private offlinePubQueue = [];
    
    public async connect(): Promise<amqp.Connection>
     {
      try {
        this.amqpConn = await connect(this.connStr);  
      } catch (error) {
        throw error;
      }

      this.amqpConn.on("error", function(err) {
        if (err.message !== "Connection closing") {
          console.error("[AMQP] conn error", err.message);
        }
      });
      this.amqpConn.on("close", function() {
        console.error("[AMQP] reconnecting");
        return setTimeout(this.start, 1000);
      });

      console.log('connected');

      return this.amqpConn;
      // this.startPublisher();
      // this.startWorker();
    }

    public start() {
        amqp.connect(this.connStr + "?heartbeat=60", function(err, conn: amqp.Connection) {
            if (err) {
              console.error("[AMQP]", err.message);
              return setTimeout(this.start, 1000);
            }
            conn.on("error", function(err) {
              if (err.message !== "Connection closing") {
                console.error("[AMQP] conn error", err.message);
              }
            });
            conn.on("close", function() {
              console.error("[AMQP] reconnecting");
              return setTimeout(this.start, 1000);
            });
        
            console.log("[AMQP] connected");
            // this.amqpConn = amqp.Connection;
            this.amqpConn = conn;
        
            this.whenConnected();

          });
    }
 
    private whenConnected() {
      startPublisher();
      startWorker();
    }
    
    private startPublisher() {
      this.amqpConn.createConfirmChannel(function(err, ch) {
        if (closeOnErr(err)) return;
        ch.on("error", function(err) {
          console.error("[AMQP] channel error", err.message);
        });
        ch.on("close", function() {
          console.log("[AMQP] channel closed");
        });
    
        pubChannel = ch;
        while (true) {
          var m = offlinePubQueue.shift();
          if (!m) break;
          publish(m[0], m[1], m[2]);
        }
      });
    }

// method to publish a message, will queue messages internally if the connection is down and resend later
    private publish(exchange, routingKey, content) {
        try {
          this.pubChannel.publish(exchange, routingKey, content, { persistent: true },
            function(err, ok) {
            if (err) {
                console.error("[AMQP] publish", err);
                this.offlinePubQueue.push([exchange, routingKey, content]);
                this.pubChannel.connection.close();
            }
            });
        } catch (e) {
          console.error("[AMQP] publish", e.message);
          this.offlinePubQueue.push([exchange, routingKey, content]);
        }
    }

    private startWorker() {
        this.amqpConn.createChannel(function(err, ch) {
          if (this.closeOnErr(err)) return;
          ch.on("error", function(err) {
            console.error("[AMQP] channel error", err.message);
          });
          ch.on("close", function() {
            console.log("[AMQP] channel closed");
          });
          ch.prefetch(10);
          ch.assertQueue("jobs", { durable: true }, function(err, _ok) {
            if (this.closeOnErr(err)) return;
            ch.consume("jobs", processMsg, { noAck: false });
            console.log("Worker is started");
          });
      
          function processMsg(msg) {
            this.work(msg, function(ok) {
              try {
                if (ok)
                  ch.ack(msg);
                else
                  ch.reject(msg, true);
              } catch (e) {
                this.closeOnErr(e);
              }
            });
          }
        });
      }
      

    private work(msg, cb) {
        console.log("Got msg", msg.content.toString());
        cb(true);
    }
      

    private closeOnErr(err) {
        if (!err) return false;
        console.error("[AMQP] error", err);
        this.amqpConn.close();
        return true;
    }
      
}
