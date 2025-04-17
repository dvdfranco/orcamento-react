import React, { useCallback, useRef } from 'react';
import { Outlet } from 'react-router-dom';
import AppMenu from './AppMenu';
import { Toast } from 'primereact/toast';
import { CategoriaContextProvider } from './context/CategoriaContext';
import { CustomMemoContextProvider } from './context/CustomMemoContext';
import { LctoContextProvider } from './context/LctoContext';

interface IToast {
    showToast(severity: string, summary: string, detail?: string): void;
}

export const ToastContext = React.createContext<IToast>({} as IToast)

export const MainLayout = (): any => {
    const toast = useRef<any>(null)

    const showToast = (severity: string, summary: string, detail?: string) => {
        toast.current.show({ severity, summary, detail, life: 7000 })
    }

    return (
        <div className="layout">
            <ToastContext.Provider value={{showToast}}>
            <CategoriaContextProvider>
            <CustomMemoContextProvider>
            <LctoContextProvider>
                <AppMenu />
                <div>
                    <Outlet />
                </div>
            </LctoContextProvider>
            </CustomMemoContextProvider>
            </CategoriaContextProvider>
            </ToastContext.Provider>

            <Toast ref={toast} />
        </div>
    );
};


