import { Assessor } from "./Assessor"
import { Operation } from "./Operation"
import { User } from "./user"


export class Turn {

    id: string;
    user: User;
    operation: Operation; 
    assigned_to: Assessor;
    turn_in_line: number;
    letter: string;
    state: number;
    creationDate: Date;

    constructor(
        { id, user, operation, assigned_to, turn_in_line, letter, state, creationDate}:
        {id: string, user: User, operation: Operation, assigned_to: Assessor, turn_in_line:number, letter:string, state:number, creationDate:Date}
    ){
    this.id = id;            
    this.user = user;                 // Objeto Cliente
    this.operation = operation;       // Objeto Operación
    this.assigned_to = assigned_to;   // Asignación de operario
    this.turn_in_line = turn_in_line; // Turno en la fila
    this.state = 1;                   // 1: EN_ESPERA |2: LLAMADO | 3: ATENDIENDO | 4: FINALIZADO
    this.letter = "";                 // Letra adicional si superan los 99 clientes en fila
    this.creationDate = new Date();   // fecha de creación
  }

  // Formato completo: TG01, SD01, TGA01, etc.
  get codigoTurno() {
    return `${this.operation.code}${this.letter}${String(this.turn_in_line).padStart(2, "0")}`;
  }
}
