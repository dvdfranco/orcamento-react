import axios from "axios";

const api = axios.create({
    baseURL: `${process.env.REACT_APP_API_BASE_URL}/Categoria/`,
    responseType: 'json'
})

export default class CatApi {

    static async getAll() {
        let response = await api.get('all');

        return response.data;
    }

}