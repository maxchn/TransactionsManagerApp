export class TransactionRecord {
    id: number;
    status: string;
    type: string;
    clientName: string;
    amount: string;

    constructor() {
        this.id = -1;
        this.status = '';
        this.type = '';
        this.clientName = '';
        this.amount = '';
     }
}
