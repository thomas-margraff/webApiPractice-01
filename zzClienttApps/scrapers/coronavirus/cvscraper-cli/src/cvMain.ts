import { cvParser } from './cv-parser';
import { cvScrapeData } from './models/cv.moldel';
import { Utils } from './cv-utils';
import moment from 'moment';
import { cvLogger } from './cvLogger';
import { scrapeDelay } from './cv-utils';
import { Sender } from './messaging/simple/send';

export class cvMain {
    parser = new cvParser();

    public async webScrapeAxios() {
        let scrapeData: cvScrapeData = new cvScrapeData();
        try {
            scrapeData = await this.parser.webScrapeAxios();
        } catch (error) {
            cvLogger.log(error);
        }
        return Promise.resolve(scrapeData);
    }

    public async oneTimeScrape () { 
        cvLogger.log(Utils.DateNowToString() + ' begin scrape');
        this.webScrapeAxios().then(scrapeData => {
            this.parser.saveHtml(this.parser.scrapedHtml);
            this.parser.saveJson(scrapeData);
            this.parser.saveToCsv(scrapeData);
            this.sendScrapeNotification(this.parser.scrapeDataToJson(scrapeData), "importscrapejson").then( r => {
                cvLogger.log(Utils.DateNowToString() + ' complete scrape');
                cvLogger.log(''); 
            });

            // this.sendScrapeNotification(this.parser.scrapedJsonFilePath).then( r => {
            //     cvLogger.log(Utils.DateNowToString() + ' complete scrape');
            //     cvLogger.log(''); 
            // });
        });
    }

    // deprecated
    public doScrape (runenv: string) {
        let prevHeading = '';
        let delay:number = Utils.getScrapeDelay(runenv);
        cvLogger.log('first scrape at ' +  moment().add(delay, 'ms').format("h:mm A"));
        cvLogger.log('');
        setInterval(() => {
            cvLogger.log('begin scrape ' + Utils.DateNowToString() );
            this.webScrapeAxios().then(scrapeData => {
                cvLogger.log('prev ' + prevHeading);
                if (prevHeading !== scrapeData.heading) {
                    this.parser.saveHtml(this.parser.scrapedHtml);
                    this.parser.saveJson(scrapeData);
                    this.parser.saveToCsv(scrapeData)
                    cvLogger.log('curr ' + scrapeData.heading + ' changed');
                } else {
                    cvLogger.log('curr ' + scrapeData.heading + ' same');
                }
                let t = moment().add(delay, 'ms').format("h:mm A");
                cvLogger.log('next scrape at: ' + t);
                cvLogger.log('');
                prevHeading = scrapeData.heading
                this.sendScrapeNotification(this.parser.scrapedJsonFilePath).then( r => {
                    //
                });
            });
        }, delay);
    }

    private async sendScrapeNotification(msg, routingKey?) {
        let sender = new Sender();
        sender.send(msg).then(r => { });
    }

    public readFromHtmlFileAndParse(file: string) {
        var scrapeData = this.parser.readFromHtmlFileAndParse(file);
        // this.sendScrapeNotification('oneTimeScrape complete').then( r => { });
    }
}