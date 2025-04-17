import React, { useEffect, useState } from 'react';
import { Lancamento } from '../models/Models';
import LctoPrevistoApi from '../services/api/LctoPrevistoApi';
import LctoApi from '../services/api/LctoApi';

interface BaseProps {
    children: React.ReactNode;
}

interface ICustomMemoContext {
    customMemos: Lancamento[];
}

const CustomMemoContext = React.createContext<ICustomMemoContext>({} as ICustomMemoContext);

const CustomMemoContextProvider: React.FC<BaseProps> = ({children}) => {
    const [ customMemos, setCustomMemos ] = useState<Lancamento[]>([]);

    useEffect(() => {
        const doit = async () : Promise<void> => {
            let listaLctoPrevisto:Lancamento[] = await LctoPrevistoApi.getAll();
            let listaMemosTemp = await LctoApi.getCustomMemos();
            let memos: Lancamento[] = [];

            //Incluir na lista exceto os repetidos:
            listaLctoPrevisto.forEach((x: Lancamento) => {
              x.isPrevisto = true; // Isto sera util pra quando usar o autocomplete dos lancamentos
              
              if (!memos.some((y: Lancamento) => x.memo === y.memo && x.categoria.id === y.categoria.id))
                memos.push(x);
            });
      
            listaMemosTemp.forEach((x: Lancamento) => {
              if (!memos.some((y: Lancamento) => x.memo === y.memo && x.categoria.id === y.categoria.id))
                memos.push(x);
            });
      
            //sort!
            memos = memos.sort((a: Lancamento, b: Lancamento) =>
              a.memo > b.memo ? 1 : -1
            );

            setCustomMemos(memos);
        };

        doit();
    }, []);

    return (
        <CustomMemoContext.Provider value={{customMemos}}>
            {children}
        </CustomMemoContext.Provider>
    )
}

export { CustomMemoContextProvider, CustomMemoContext }