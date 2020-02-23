import cheerio from 'cheerio';
import * as fs from 'fs';
import * as path from 'path';
import axios from 'axios';
import { cvScrapeData, cvModel } from './models/cv.moldel';
import { Utils, DataDirectories } from './cv-utils';
const { Parser } = require('json2csv');
import { cvLogger } from './cvLogger';

// https://bnonews.com/index.php/2020/02/the-latest-coronavirus-cases/
// #mvp-content-main > table:nth-child(9)

export class cvParser {
    public scrapedHtml = '';
    public scrapedJsonFilePath = '';
    public scrapedHtmlFilePath = '';

    async getHtmlFromWeb() {
        let url = 'https://bnonews.com/index.php/2020/02/the-latest-coronavirus-cases/';
        try {
           let res = await axios({
                url: url,
                method: 'get',
                timeout: 8000,
                headers: {
                    'Content-Type': 'application/json',
                }
            })
            if(res.status == 200){
            }    
            // Don't forget to return something   
            return res.data;
        }
        catch (err) {
            cvLogger.log(err);
        }
    }

    async webScrapeAxios() {
        let scrapeData: cvScrapeData = new cvScrapeData();

        let url = 'https://bnonews.com/index.php/2020/02/the-latest-coronavirus-cases/';
        try {
           let res = await axios({
                url: url,
                method: 'get',
                timeout: 8000,
                headers: {
                    'Content-Type': 'application/json',
                }
            })
            if(res.status == 200){
            }    

            // Don't forget to return something   
            scrapeData = this.parse(res.data);
        }
        catch (err) {
            cvLogger.log(err);
        }

        return Promise.resolve(scrapeData);
        
    }

    readFromHtmlFileAndParse(filePath: string) {
        let html = '';
        try {
            html = this.getHtmlFromFile(filePath);    
            return this.parse(html);
        } catch (error) {
            throw error;
        }
    }

    getHtmlFromFile(filePath: string): string {
        let html = '';
        try {
            if (fs.existsSync(filePath)) {
                html = fs.readFileSync(filePath).toString();
            } else {
                throw filePath + ' doesnt exist';
            }
        }
        catch (error) {
            cvLogger.log(error);
            throw (error);
        }
        return html;
    }

    parse(html: string): cvScrapeData {
        this.scrapedHtml = html;
        const $ = cheerio.load(html);
        let cvScrape = new cvScrapeData();
        let sd = cvScrape.geoLocations;

        // #mvp-content-main > p:nth-child(2) > strong
        let heading = $('#mvp-content-main > p:nth-child(2) > strong').text();
        cvScrape.heading = heading;

        // MAINLAND CHINA
        let selector = '#mvp-content-main > table.wp-block-table.aligncenter.is-style-stripes > tbody > tr';
        parseHtmlTable(selector, 'Mainland China')
        
        // REGIONS
        selector = '#mvp-content-main > table:nth-child(9) > tbody > tr'
        
        parseHtmlTable(selector, 'Regions')
        
        // INTERNATIONAL
        selector = '#mvp-content-main > table:nth-child(11) > tbody > tr'
        parseHtmlTable(selector, 'International')
        
        function parseHtmlTable(selector: string, name: string) {
            let isFirst = true;
            let location = new cvModel();
            location.name = name;
    
            $(selector).each(function(index, element: CheerioElement) {    
                // skip header
                if (isFirst) {
                    isFirst = false;
                } else {
                    const children = $(element).children();
                    location.details.push({
                        country: $(children[0]).text().trim(),
                        cases: $(children[1]).text().trim(),
                        deaths: $(children[2]).text().trim(),
                        notes: $(children[3]).text().trim()
                    });
                }
            });
    
            sd.push(location);
        }
        return cvScrape;
    }

    saveHtml(html: string) {
        let fname = Utils.DateToString(new Date()) + '-bno.html'
        let fpath = path.join(DataDirectories.HTML_DIR, fname);
        this.scrapedHtmlFilePath = fpath;
        fs.writeFile(fpath, html, (err) => {
    		if (err) {
                cvLogger.log('an error happened while saving the html file ' + err);
                throw (err);
    		}
    	});
    }

    saveJson(scrapeData: cvScrapeData) {
        // write to json file
        let fname = Utils.DateToString(scrapeData.scrapeDate) + '-bno.json'
        let fpath = path.join(DataDirectories.JSON_DIR, fname);
        this.scrapedJsonFilePath = fpath;

        fs.writeFile(fpath, JSON.stringify(scrapeData, null, 2), (err) => {
    		if (err) {
                cvLogger.log('an error happened while saving the json file ' + err);
                throw (err);
    		}
    	});
    }    

    saveToCsv(scrapeData: cvScrapeData) {
        const fields = ['geolocation', 'scrapeDate', 'country', 'cases', 'deaths', 'notes'];
        const opts = { fields };
        let csv = '';
        try {
            const parser = new Parser(opts);
            csv = parser.parse(scrapeData.toCsv());
        } catch (err) {
            cvLogger.log(err);
        }

        let fname = Utils.DateToString(scrapeData.scrapeDate) + '-bno.csv'
        let fpath = path.join(DataDirectories.CSV_DIR, fname);
        fs.writeFile(fpath, csv, (err) => {
            if (err) {
                cvLogger.log('an error happened while saving the json file ' + err);
                throw (err);
            }
        });
    }
    
    scrapeDataToJson(scrapeData: cvScrapeData): string {
        return JSON.stringify(scrapeData, null, 2)
    }
}        