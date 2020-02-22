import moment from 'moment';
import { cvParser } from './cv-parser';
import { cvLogger } from './cvLogger';
import * as path from 'path';
import * as fs from 'fs';

export enum scrapeDelay {
    FifteenSeconds = 15000,
    HalfHour = 1800000,
    Hour = 3600000,
    TenMinutes = 600000
}

export enum runEnv {
    dev = 'dev',
    test = 'test',
    prod = 'prod'
}

export class DataDirectories {
    public static CSV_DIR = 'C:\\devApps\\typescriptscraper\\coronavirus\\data\\csv';
    public static JSON_DIR = 'C:\\devApps\\typescriptscraper\\coronavirus\\data\\json';
    public static HTML_DIR = 'C:\\devApps\\typescriptscraper\\coronavirus\\data\\html';

    static checkFoldersExist() {
        DataDirectories.createDirectortIfNotExists(DataDirectories.CSV_DIR);
        DataDirectories.createDirectortIfNotExists(DataDirectories.JSON_DIR);
        DataDirectories.createDirectortIfNotExists(DataDirectories.HTML_DIR);
    }

    static createDirectortIfNotExists(dirName: string)  {
        try {
            if (fs.existsSync(dirName)) {
                return;
             } else {
                fs.mkdirSync(dirName, {recursive: true});
             }
        } catch (error) {
            throw error;            
        }
    }
}

export class Utils {

    static getScrapeDelay(env: string): number {
        /* 
            15000   - 15 seconds
            1800000 - 30 minutes
            600000  - 10 minutes
            3600000 - 1 hour
        */
        if (env === runEnv.dev) {
            return scrapeDelay.FifteenSeconds;
        } else if( env === runEnv.prod) {
            return scrapeDelay.HalfHour;
        } else if( env === runEnv.test) {
            return scrapeDelay.TenMinutes;
        } else {
            throw 'bad runEnv = ' + runEnv
        }
    }

    static DateToString(dtc): string {
        let dt = moment(dtc);
        let dtStr = dt.format("YYYY.MM.DD-HHmm");
        return dtStr;
    }

    static DateNowToString(): string {
        let dt = moment();
        let dtStr = moment().format("MM/DD/YYYY hh:mm A");
        return dtStr;
    }

    static webScrapeFromFile() {
        let parser = new cvParser();
        let filePath = path.resolve(__dirname, 'bnonews.html');
        
        try {
            const scrapeData = parser.readFromHtmlFileAndParse(filePath);    
            parser.saveJson(scrapeData);
            parser.saveToCsv(scrapeData); 
            cvLogger.log('done');
        } catch (error) {
            cvLogger.log(error);
        }
    }

    static async getHtmlFromWeb() {
        let parser = new cvParser();
        try {
            let html =  await parser.getHtmlFromWeb();
            Utils.parseHtml(html);
        } catch (error) {
            cvLogger.log(error);
        }
    }

    static parseHtml(html: string) {
        let parser = new cvParser();
        try {
            let scrapeData = parser.parse(html);
            parser.saveJson(scrapeData);
            parser.saveToCsv(scrapeData); 
            cvLogger.log('done');
    
        } catch (error) {
            cvLogger.log(error);
        }
    }
        
    static getHtmlFromFile() {
        let parser = new cvParser();
        let filePath = path.resolve(__dirname, 'bnonews.html');
        try {
            let html = parser.getHtmlFromFile(filePath);
            Utils.parseHtml(html);
        } catch (error) {
            cvLogger.log(error);
        }
    }
       
}