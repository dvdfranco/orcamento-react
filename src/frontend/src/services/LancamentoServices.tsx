import moment from 'moment';
import React from 'react';
import { Lancamento } from '../models/Models';
import LctoApi from './api/LctoApi';

export default class LancamentoServices {

    static async dividir(itens: Lancamento[], lista: Lancamento[]) {
        let itensNovos = await LctoApi.dividir(itens);

        //remover o item original da lista:
        lista = lista.filter((x) => (x.id !== itens[0].id));
        //e adicionar os novos divididos:
        itensNovos.forEach((x: Lancamento) => { lista.push(x); });

        return lista;
    }

    static async delete(lcto: Lancamento, lista: Lancamento[]) {
        await LctoApi.delete(lcto as Lancamento);
        return lista.filter((x) => (x.id !== lcto.id));
    }

    static async update(lcto: Lancamento, lista: Lancamento[], itemPrevistoOriginal?: string) {
        await LctoApi.update(lcto);

        // Se o lancamento mudou pra previsto, remover da lista de previstos e readicionar este item:
        if (lcto.isPrevisto)
        {
            lista = lista.filter(x => !(x.isPrevisto && x.memo === lcto.memo && x.idCategoria === lcto.idCategoria));
            lista = [...lista, lcto];
        }
        // Se removeu dos previstos, o item vai mover pra lista nova, entao melhor recriar o previsto original:
        else if ((itemPrevistoOriginal ?? "") !== "")
        {
            const devolverPrevisto = {
                ...lcto,
                transid: 0,
                id: `${lcto.id}-old`,
                memo: itemPrevistoOriginal ?? "",
                isPrevisto: true
            }
            lista = [...lista, devolverPrevisto];
        }

        return lista;
    }

    static async updateDataRef(lcto: Lancamento, addMes: number, lista: Lancamento[]) {
        lcto.dataRef = moment(lcto.dataRef).add(addMes, 'M').toDate();
        await LctoApi.update(lcto);
        return lista.filter((x) => (x.id !== lcto.id));
    }

}