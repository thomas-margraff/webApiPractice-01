import { cvMain } from './cvMain';

export class MessageProcessor {
    public static async HandleMessage(msg ) {
        let main: cvMain = new cvMain();
        let message = msg.content.toString()
        if (msg === 'newscrape') {
            // console.log("processing", message)
            // main.oneTimeScrape();
        }
        if (msg.fields.routingKey === 'doscrape') {
            console.log("processing", message)
            main.oneTimeScrape();
        }

        if (msg.fields.routingKey === 'cv.scraper.queue.doscrape') {
            console.log("processing", message)
            main.oneTimeScrape();
        }
        
    }
}