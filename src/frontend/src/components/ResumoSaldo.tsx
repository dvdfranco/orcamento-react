import React, { useEffect, useRef, useState } from 'react'
import { Card } from 'primereact/card';
import { Tag } from 'primereact/tag';
import { ProgressSpinner } from 'primereact/progressspinner';
import InputMoney from './form/InputMoney';
import moment from 'moment';
import SaldoAnteriorApi from '../services/api/SaldoApi';

interface Props {
    dataRef: Date,
    Saldos: SaldoCalc
    onUpdate: () => void
}

interface SaldoCalc {
    saldoHoje: number;
    dataSaldoHoje: number;
    saldoAnterior: number;
    soma: number;
    saldoCalculado: number;
    saldoPrevisto: number;
}


const ResumoSaldo: React.FC<Props> = (props) => {
    const [ editSaldo, setEditSaldo ] = useState<boolean>(false);
    const [ loading, setLoading ] = useState<boolean>(false);
    const inputRef = useRef<any>(null);

    const onKeyUp = (e:any) => 
    {
        if (e.key === "Enter")
        {
            const strValor = e.target?.value;

            const valor = Number(strValor);

            if (isNaN(valor))
                return;

            setLoading(true);
            SaldoAnteriorApi.updateSaldoAnterior({
                mesRef: moment(props.dataRef).format("YYYY-MM-DD"), // moment(props.dataRef).format("YYYYMM"),
                valor: valor
            })
            .then(() => {
                props.onUpdate()
                setLoading(false);
                setEditSaldo(false);
            });
        }
        else if (e.code === "Escape")
        {
            setEditSaldo(false);
        }
    }

    useEffect(() => {
        setEditSaldo(false);
    }, [props]);

    return (
        <Card style={{backgroundColor: 'var(--primary-100)'}}>
            <div className="flex flex-column">
                <div className='flex flex-row'>
                    <div className='flex justify-content-left col-9'>Saldo Anterior</div>
                    <div className='flex align-content-right col-3'>
                        {editSaldo ? (
                            <>
                            <InputMoney
                                key={props.dataRef.toISOString()}
                                name="txtSaldoAnterior"
                                value={props.Saldos.saldoAnterior}
                                onKeyUp={onKeyUp}
                                inputRef={inputRef}
                                singleLine
                            />
                            {loading && <ProgressSpinner style={{width: '30px', height:'30px'}} />}
                          </>
                        ) : (
                            <Tag severity={props.Saldos.saldoAnterior > 0 ? "success" : "danger"} value={`R$ ${props.Saldos.saldoAnterior.toFixed(2)}`} 
                                onDoubleClick={() => { setEditSaldo(true); 
                                    //TODO: selecionar texto do input nao funciona mais apos eu criar o InputNumber novo
                                    window.setTimeout(() => { inputRef.current?.select(); }, 150); } } />
                        )}
                    </div>
                </div>
                <div className='flex flex-row'>
                    <div className='flex justify-content-left col-9'>Recebs - Pagtos</div>
                    <div className='flex align-content-right col-3'><Tag severity={props.Saldos.soma > 0 ? "success" : "danger"} value={`R$ ${props.Saldos.soma.toFixed(2)}`} /></div>
                </div>

                <div className='flex flex-row'>
                    <div className='flex justify-content-left col-9'>Saldo Calculado</div>
                    <div className='flex align-content-right col-3' title="Usar este saldo para o Anterior do proximo mes">
                        <Tag severity={props.Saldos.saldoCalculado > 0 ? "success" : "danger"} value={`R$ ${props.Saldos.saldoCalculado.toFixed(2)}`} />
                    </div>
                </div>
                {(moment(props.Saldos.dataSaldoHoje).format("YYYY-MM-DD") == moment().format("YYYY-MM-DD")) &&
                <div className='flex flex-row'>
                    <div className='flex justify-content-left col-9'>Saldo Hoje</div>
                    <div className='flex align-content-right col-3'><Tag severity={props.Saldos.saldoHoje > 0 ? "success" : "danger"} value={`R$ ${props.Saldos.saldoHoje.toFixed(2)}`} /></div>
                </div>}
                <div className='flex flex-row'>
                    <div className='flex justify-content-left col-9'>Saldo Previsto (fim do mês)</div>
                    <div className='flex align-content-right col-3' title="Saldo considerando os previstos que ainda não foram pagos">
                        <Tag severity={props.Saldos.saldoPrevisto > 0 ? "success" : "danger"} value={`R$ ${props.Saldos.saldoPrevisto.toFixed(2)}`} />
                    </div>
                </div>
            </div>
        </Card>
    );
}

export default ResumoSaldo;