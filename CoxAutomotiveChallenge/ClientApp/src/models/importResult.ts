export interface ImportResult {
    rowNo: number;
    performedAction: string;
    message: string;
    deal: Deal;
}


export interface Deal {
    dealNumber: number;
    customerName: string;
    dealershipName: string;
    vehicle: string;
    price: number;
    date: Date;
}

export interface ImportSummary {
    importResults: ImportResult[];
    importErrors: ImportErrors[];
    summary: Summary[];
}

export interface Summary {
    itemName: string;
    itemDescription: string;
    itemValue: string;
}

export interface ImportErrors {
    rowNo: string;
    performedAction: string;
    message: string;
}

