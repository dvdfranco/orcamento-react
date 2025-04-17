import { Container, Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import React, { useContext, useEffect } from 'react';
import { useState } from 'react';
import { Categoria, Lancamento } from '../models/Models';
import ItemLancamento from './ItemLancamento';
import moment from 'moment';
import styles from './ListaLancamento.module.css';
import NumberFormat from 'react-number-format';
import ItemMenu from './ItemMenu';
import { LctoContext } from '../context/LctoContext';

interface Props {
    filtroCategoria?: Categoria | null;
    somentePrevistos: boolean;
    handleChange: (data: any, tipoAlteracao: string, itemPrevistoOriginal?: string) => void;
    handlePadraoChange: (data: any) => void;
}

const ListaLancamento: React.FC<Props> = (props) => { 

    const lctoContext = useContext(LctoContext);
    const [listaLancamento, setListaLancamento] = useState<Lancamento[]>([]);

    useEffect(() => {
        if (!lctoContext.listaLcto?.length)
            return;

        setListaLancamento(lctoContext.listaLcto
            .filter(x => x.isPrevisto === props.somentePrevistos && (x.categoria.id === (props.filtroCategoria?.id ?? x.categoria.id)))
        );

    }, [lctoContext.listaLcto, props.filtroCategoria, props.somentePrevistos])

    return (
        <Grid container>
            <Grid item xs={10}>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell><b>Cat.</b></TableCell>
                                <TableCell><b>Data</b></TableCell>
                                <TableCell><b>Memo</b></TableCell>
                                <TableCell><b>Valor</b></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {listaLancamento
                                .map((lcto: Lancamento) => (
                                    <TableRow key={lcto.id}>
                                        <TableCell><ItemMenu  cat={lcto.categoria} item={lcto} handleChange={props.handleChange} handlePadraoChange={props.handlePadraoChange}/></TableCell>
                                        <TableCell>{moment(lcto.data).format('DD/MM')}</TableCell>
                                        <TableCell title={lcto.bankMemo}><ItemLancamento item={lcto} handleChange={props.handleChange} /></TableCell>
                                        <TableCell className={styles.right}
                                            ><NumberFormat 
                                                displayType='text' fixedDecimalScale={true} value={lcto.valor} decimalScale={2} 
                                                className={lcto.transid === 0 ? styles.naopago : (lcto.valor >=0 ? styles.positivo : styles.negativo)}
                                        /></TableCell>
                                    </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Grid>
        </Grid>
    );
}

export default ListaLancamento;