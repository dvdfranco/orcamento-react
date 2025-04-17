import React, { useEffect, useState } from 'react';
import { Lancamento } from '../models/Models';
import LctoPrevistoApi from '../services/api/LctoPrevistoApi';
import LctoApi from '../services/api/LctoApi';

interface BaseProps {
    children: React.ReactNode;
}

interface ILctoContext {
    listaLcto: Lancamento[];
    listaLctoPrevisto: Lancamento[];
    updateListaLcto: (lctos: Lancamento[]) => void;
    updateListaLctoFromData: (dataRef: moment.Moment) => void;
}

const LctoContext = React.createContext<ILctoContext>({} as ILctoContext);

const LctoContextProvider: React.FC<BaseProps> = ({children}) => {
    const [ listaLctoPrevisto, setListaLctoPrevisto ] = useState<Lancamento[]>([]);
    const [ listaLcto, setListaLcto ] = useState<Lancamento[]>([]);

    useEffect(() => {
        const doit = async () : Promise<void> => {
            setListaLctoPrevisto(await LctoPrevistoApi.getAll());
        };

        doit();
    }, []);

    const updateListaLcto = (lctos: Lancamento[]) => {
        setListaLcto(lctos);
    }

    const updateListaLctoFromData = (dataRef: moment.Moment) => {
        LctoApi.getLancamentos(dataRef.format("YYYYMM")).then((data) => {
            setListaLcto(data);
          });
    };

    return (
        <LctoContext.Provider value={{listaLcto, listaLctoPrevisto, updateListaLctoFromData, updateListaLcto}}>
            {children}
        </LctoContext.Provider>
    )
}

export { LctoContextProvider, LctoContext }
