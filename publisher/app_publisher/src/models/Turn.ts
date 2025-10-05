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
    this.id = id;                   // UUID o contador
    this.user = user;         // Objeto Cliente
    this.operation = operation;       // Objeto Servicio
    this.assigned_to = assigned_to;           // Número de turno (01, 02, …)
    this.turn_in_line = turn_in_line;             // Letra adicional si hay más de 99
    this.letter = letter;           // 1: EN_ESPERA |2: LLAMADO | 3: ATENDIENDO | 4: FINALIZADO
    this.state = state;     // ID o nombre del asesor
    this.creationDate = new Date();
  }

  // Formato completo: TG01, SD01, TGA01, etc.
  get codigoTurno() {
    return `${this.operation.code}${this.letter}${String(this.turn_in_line).padStart(2, "0")}`;
  }
}
