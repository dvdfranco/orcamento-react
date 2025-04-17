import { MemoPadrao } from '../models/Models';
import MemoPadraoApi from './api/MemoPadraoApi';

export default class MemoPadraoServices {

    static async add(data: MemoPadrao) {
        MemoPadraoApi.add(data);
    }
 
}