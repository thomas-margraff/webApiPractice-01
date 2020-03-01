import { MessageProcessor } from './../../messageProcessor';
import amqp from "amqplib/callback_api"; 

export class Receiver {
    private connStr = 'amqp://noekmbda:jtKtg3LU2uEQOCJRpEgMhdSqf2N7S_Cv@gull.rmq.cloudamqp.com/noekmbda';
    private localHost = 'amqp://localhost'

    constructor() {}

    public async receive() {
        amqp.connect(this.connStr, function(error0, connection) {
            if (error0) {
                throw error0;
            }
            connection.createChannel(function(error1, channel) {
                if (error1) {
                    throw error1;
                }

                let exchange = 'cv.scraper.exchange';
                let queue = '';

                channel.assertExchange(exchange, 'direct', {
                    durable: true
                });   
                
                //channel.assertQueue(queue, { durable: true });
                channel.assertQueue(queue, {
                    exclusive: false
                }, function(error2, q) {
                    if (error2) {
                        throw error2;
                    }
                    queue = q.queue;
                    channel.bindQueue(q.queue, exchange, 'doscrape');
                    channel.consume(q.queue, function(msg) {
                    // console.log(" [x] %s: '%s'", msg.fields.routingKey, msg.content.toString());
                    MessageProcessor.HandleMessage(msg);
                }, {
                    noAck: true
                });
            });
                
            console.log(" [*] 1 Waiting for messages in %s. To exit press CTRL+C", queue);
            });
        });
    }
}