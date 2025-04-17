import axios from "axios";
import { MemoPadrao } from "../../models/Models";

const api = axios.create({
    baseURL: `${process.env.REACT_APP_API_BASE_URL}/MemoPadrao/`,
    responseType: 'json'
})

export default class MemoPadraoApi {

    static async add(data: MemoPadrao) {
        let response = await api.post('', data);

        return response.data;
    }

}