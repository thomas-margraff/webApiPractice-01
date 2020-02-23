#!/usr/bin/env node

// C:\devApps\typescriptscraper\coronavirus\cvscraper-cli
// npm install -g @tmargraff/cvscraper-cli
// npm uninstall -g @tmargraff/cvscraper-cli
// cvscraper-cli

import { cvLogger } from './cvLogger';
import { cvMain } from './cvMain';
import { Utils, DataDirectories } from './cv-utils';
import { scrapeDelay } from './cv-utils';
import { runEnv } from './cv-utils';
import * as path from 'path';
import { cvMessagingRmq } from './messaging/cvMessagingRmq';
import { Sender } from './messaging/simple/send';
import { Receiver } from './messaging/simple/receive';
import moment from 'moment';

export class Main {
    static log(val:any) {
        console.log(val);
    }    
}

let debug = false;

//#region testing
// if (debug) {
//     let sender = new Sender();
//     sender.send('message 1', 'test').then(r => { });
//     sender.send('message 2', 'test').then(r => { });
// }

// let fname = Utils.DateToString(Utils.DateNowToString()) + '-bno.html'
// let fpath = path.join(DataDirectories.HTML_DIR, fname);
// console.log(fname);
// let t = moment().add(1800000, 'ms').format("h:mm A");
// console.log(t);

// console.log(Utils.DateNowToString());
// console.log(scrapeDelay.HalfHour);
// console.log(runEnv.dev);
// console.log(runEnv.prod);

// let fpath = path.join(DataDirectories.HTML_DIR, '2020.02.12-2142-bno.html');
// main.readFromHtmlFileAndParse(fpath);

//#endregion testing

// wait for rmq notification
let receiver: Receiver = new Receiver();
receiver.receive().then(r => {
    console.log('waiting');
});

if (!debug) {
    // ensure csv and json folders exist
    try {
        DataDirectories.checkFoldersExist();    
    } catch (error) {
        console.error(error);
        process.exit(1);
    }
    
    // do 1 initial scrape on startup
    let main: cvMain = new cvMain();
    main.oneTimeScrape();
}