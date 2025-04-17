import axios from "axios";

const api = axios.create({
    baseURL: `${process.env.REACT_APP_API_BASE_URL}/Saldo/`,
    responseType: 'json'
})

interface Saldo {
    valor: number;
    mesRef: string;
}
export default class SaldoApi {

    static async updateSaldoAnterior(saldo: Saldo) {
        let response = await api.put('UpdateSaldoAnterior', {
            valor: saldo.valor,
            data: saldo.mesRef
        });
        return response.data;
    }

}