import React, { useContext, useEffect, useState } from "react";
import { FilterMatchMode } from 'primereact/api'
import { DataTable } from 'primereact/datatable'
import { Column } from 'primereact/column'
import { InputSwitch } from 'primereact/inputswitch'
import { Lancamento } from "../models/Models";
import LctoApi from "../services/api/LctoApi";
import { ToastContext } from "../MainLayout";

const Memo: React.FC = () => {
    const [ listaItens, setListItens] = useState<Lancamento[]>([]);
    const [ searchAutocompleted, setsearchAutocompleted] = useState(true);
    const toast = useContext(ToastContext);

    useEffect(()=> {
        LctoApi.getCustomMemos(searchAutocompleted).then((data) => {
            setListItens(data);
        });
    }, [searchAutocompleted]);

    const editButton = (rowData:Lancamento) => {
        return (<InputSwitch checked={rowData.autocomplete} onChange={(e) => updateAutocomplete(rowData.memo, e.value ?? false)} />);
    }

    const updateAutocomplete = (memo: string, autocomplete: boolean) =>
    {
        LctoApi.updateCustomMemo(memo, autocomplete).then(() => {
            toast.showToast("success", "Atualizado!");

            const lctos = listaItens.map(x => {
                if (x.memo === memo)
                    x.autocomplete = autocomplete;

                return x;
            })

            setListItens(lctos);
        })
    }

    return (<div className="ml-7">
        <div className="grid flex justify-content-start mt-5">
            <div className="col-2">
                Buscar Autocomplete:
            </div>
            <div className="col-1">
                <InputSwitch className={'flex flex-row'} checked={searchAutocompleted} onChange={(e) => setsearchAutocompleted(!searchAutocompleted)} />
            </div>
        </div>
        <div className="col-4 flex justify-content-start align-items-center align-center p-0">
            <DataTable value={listaItens} className="mt-5 col-9 "
                dataKey='memo'
                filterDisplay="row"
                filters={{ global: { value: null, matchMode: FilterMatchMode.CONTAINS }, memo: { value: null, matchMode: FilterMatchMode.CONTAINS }}}
                emptyMessage="Nada encontrado"
                >
                <Column className="" filter field="memo" header="Memo" />
                <Column className="col-1" field="autocomplete" header="Autocomplete?" body={editButton} />
            </DataTable>
        </div>
    </div>
    );
}

export default Memo;
