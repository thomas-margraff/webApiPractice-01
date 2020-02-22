import { Utils } from './cv-utils';
import { Sender } from './messaging/simple/send';

export class cvLogger {
    public static async log(msg: string) {
        console.log(msg);
        // let s = new Sender();
        // s.send(msg, 'cvConsoleLogger')
        // .then(r =>{
        //     // console.log('start scrape service', Utils.DateNowToString());
        // });
    }
}