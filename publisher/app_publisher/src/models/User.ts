export class User{
    id: string;
    name: string;
    assitance: boolean;
    document : string;

    constructor(
        {id, name, document, assitance = false} : {id:string, name:string, document:string, assitance: boolean}
    ){
        this.id = id;
        this.name = name;
        this.document = document;
        this.assitance = assitance;

    }
}

