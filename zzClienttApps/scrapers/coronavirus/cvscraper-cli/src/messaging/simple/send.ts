#!/usr/bin/env node
import amqp from "amqplib"; 
import { cvLogger } from '../../cvLogger';
// import amqp from "amqplib/callback_api"; 

export class Sender {
    private connStr = 'amqp://noekmbda:jtKtg3LU2uEQOCJRpEgMhdSqf2N7S_Cv@gull.rmq.cloudamqp.com/noekmbda';

    public async send (msg: string, routingKey?: string) {
        amqp.connect(this.connStr).then(function(conn: any) {
            if(!routingKey) {
                routingKey = 'importscrape';
            }
            conn.createChannel().then(function(channel: any) {
                let exchange = 'cv.scraper.exchange';
    
                channel.assertExchange(exchange, 'direct', {
                    durable: true
                });

                channel.publish(exchange, routingKey, Buffer.from(msg));
               
            });
            setTimeout(function() {
                conn.close();
            }, 500);
        });
    }        
}
