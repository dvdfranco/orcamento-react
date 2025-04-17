import React, { useEffect, useState } from 'react';
import { Categoria } from '../models/Models';
import CatApi from '../services/api/CatApi';

interface BaseProps {
    children: React.ReactNode;
}

interface ICategoriaContext {
    categorias: any;
}

const CategoriaContext = React.createContext<ICategoriaContext>({} as ICategoriaContext);

const CategoriaContextProvider: React.FC<BaseProps> = ({children}) => {
    const [ categorias, setCategorias ] = useState<Categoria[]>([]);

    useEffect(() => {
        CatApi.getAll().then((data) => {
            setCategorias(data.filter((x: Categoria) => x.tipo === 0));
        });
    }, []);

    return (
        <CategoriaContext.Provider value={{categorias}}>
            {children}
        </CategoriaContext.Provider>
    )
}

export { CategoriaContextProvider, CategoriaContext }