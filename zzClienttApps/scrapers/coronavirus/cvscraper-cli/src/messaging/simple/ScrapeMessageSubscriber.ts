import { MessageProcessor } from '../../messageProcessor';
import amqp from "amqplib/callback_api"; 

export class ScrapeMessageSubscriber {
    private connStr = 'localhost';
    
    constructor() {}

    public async receive() {
        amqp.connect('amqp://localhost', function(error0, connection) {
            if (error0) {
                throw error0;
            }
            connection.createChannel(function(error1, channel) {
                if (error1) {
                    throw error1;
                }
                let queue = 'cv.scraper.queue.doscrape';
                channel.assertQueue(queue, {
                    durable: true,
                    exclusive: false
                }, function(error2, q) {
                    if (error2) {
                        throw error2;
                    }
                    queue = q.queue;
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