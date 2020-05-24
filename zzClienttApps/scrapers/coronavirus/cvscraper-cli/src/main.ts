#!/usr/bin/env node

// C:\devApps\typescriptscraper\coronavirus\cvscraper-cli
// npm install -g @tmargraff/cvscraper-cli
// npm uninstall -g @tmargraff/cvscraper-cli
// cvscraper-cli

import { ScrapeMessageSubscriber } from './messaging/simple/ScrapeMessageSubscriber';
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
        alert(val);
    }    
}
function doSomething() {
    alert("in main");
}
// wait for rmq notification
let receiver: Receiver = new Receiver();
receiver.receive().then(r => {
    console.log('receiver waiting');
});

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
