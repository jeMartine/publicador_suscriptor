export class Assessor{
    id: string;
    first_name: string;
    last_name: string;
    document: string;
    area: string;

    constructor(
        {id, first_name, last_name, document, area} : {id:string, last_name:string, first_name:string,  document:string, area:string}
    ){
        this.id = id;
        this.first_name = first_name;
        this.last_name = last_name;
        this.document = document;
        this.area = area;

    }
}

