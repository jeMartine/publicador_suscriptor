export class Operation{
    id: string;
    code: string;
    name: string;
    category : string;

    constructor(
        {id, name, code, category} : {id:string, name:string, code:string, category: string}
    ){
        this.id = id;
        this.name = name;
        this.category = category;
        this.code = code;

    }
}

