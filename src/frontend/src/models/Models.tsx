import React from 'react';

export interface Lancamento {
    id: string;
    idConta: string;
    idLancamentoRef: string;
    transid: number;
    valor: number;
    valorOriginal: number;
    data: Date;
    dataRef: Date;
    autocomplete: boolean;
    tipo: number;
    memo: string;
    bankMemo: string;
    idCategoria: string;
    categoria: Categoria;
    //conta: Conta;
    isPrevisto: boolean;
    idLancamentoPrevisto: string;
    dataInclusao: Date;
}


export interface Categoria {
    id: string;
    descricao: string;
    icone: string;
    cor: string;
    tipo: number, //Enum?
}

export interface MemoPadrao {
    from: string;
    to: string;
    tipo: string;
    idCategoria: string;
}