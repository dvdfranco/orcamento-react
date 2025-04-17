import axios from "axios";
import moment from "moment";

const api = axios.create({
    baseURL: `${process.env.REACT_APP_API_BASE_URL}/LancamentoPrevisto/`,
    responseType: 'json'
})

export default class LctoPrevistoApi {

    static async getAll() {
        let response = await api.get('');

        return response.data;
    }

}