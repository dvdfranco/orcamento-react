import axios from "axios";
import moment from "moment";
import { Lancamento } from "../../models/Models";

const api = axios.create({
    baseURL: `${process.env.REACT_APP_API_BASE_URL}/Lancamento/`,
    responseType: 'json'
})

export default class LctoApi {

    static async getLancamentos(data: String) {
        let dataIni = moment(data + "01", 'YYYYMMDD');
        let dataFim = moment(dataIni).add(1, 'M').add(-1, 'd');

        let response = await api.post('search', {
            mesFrom: dataIni.format('YYYY-MM-DD'),
            mesTo: dataFim.format('YYYY-MM-DD'),
            Tipo: 0,
            IncludePrevistos: true
        });

        return response.data;
    }

    static async getCustomMemos(autoComplete: boolean | null = true) {
        let response = await api.get(`searchCustomMemos?autoComplete=${autoComplete ?? ''}`);

        return response.data;
    }

    static async getCalcSaldos(data: String) {
        let response = await api.get(`CalcSaldos/${data}`);
        return response.data;
    }

    static async delete(item: Lancamento) {
        let response = await api.delete('?Id=' + item.id);
        return response.data;
    }

    static async update(item: Lancamento) {
        let response = await api.put('', item);
        return response.data;
    }

    static async updateCustomMemo(memo: string, autocomplete: boolean) {
        let response = await api.put('PutAutocomplete', {
            memo,
            bankMemo: memo,
            autocomplete
        });

        return response.data;
    }

    static async dividir(itens: Lancamento[]) {
        let response = await api.put('dividir', itens);
        return response.data;
    }

    static async getEstatisticas(data: String) {
        let response = await api.get(`GetEstatisticas/${data}`);
        return response.data;
    }

}