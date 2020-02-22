export class cvDetail {
    public country = '';
    public cases = '';
    public deaths = '';
    public notes = '';
}

export class cvDetailDto {
    public scrapeDate: Date;
    public geolocation = '';
    public country = '';
    public cases = '';
    public deaths = '';
    public notes = '';

    constructor() {
        this.scrapeDate = new Date();
    }
    public create(scrapeDate: Date, geolocation: string, detail: cvDetail) {
        this.scrapeDate = scrapeDate;
        this.geolocation = geolocation;
        this.country = detail.country;
        this.cases = detail.cases;
        this.deaths = detail.deaths;
        this.notes = detail.notes;
        return this;
    }
}

export class cvModel {
    public name = '';
    public details: Array<cvDetail>;
    constructor() {
        this.details = new Array<cvDetail>();
    }
}

export class cvScrapeData {
    public scrapeDate: Date;
    public heading = '';
    public geoLocations: Array<cvModel>;
    constructor() {
        this.scrapeDate = new Date();
        this.geoLocations = new Array<cvModel>();
    }

    public toCsv(): Array<cvDetail> {
        let details = new Array<cvDetailDto>();
        this.geoLocations.forEach(geo => {
            geo.details.forEach(r => {
                let dto = new cvDetailDto().create(this.scrapeDate, geo.name, r);
                details.push(dto);
            });
        })
        return details;
    }
}