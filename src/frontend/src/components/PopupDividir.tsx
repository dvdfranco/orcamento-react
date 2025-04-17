import { Box, Button, Chip, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Grid, Icon, Paper, PaperProps, TextField, ToggleButton, ToggleButtonGroup, Typography } from '@mui/material';
import React, { useEffect, useRef, useState } from 'react'
import { FormatAlignLeftTwoTone, FormatAlignCenterTwoTone, FormatAlignRightTwoTone, ArrowRightTwoTone, AddBoxTwoTone } from '@mui/icons-material';
import Draggable from 'react-draggable';
import { DialogPaperComponent, IconeCategoria } from '../utils/Utils';
import { Lancamento } from '../models/Models';
import { useForm } from 'react-hook-form'

interface Props {
    item: Lancamento,
    onChange: (data: any, tipoAlteracao: string) => void,
    handleOpen: (data: boolean) => void,
    open: boolean
}


const PopupDividir: React.FC<Props> = (props) => {
    const [open, setOpen] = useState(false);
    const inputRef = useRef<HTMLInputElement | null>(null);
    const [newItems, setNewItems] = useState<any[]>([]);
    const [valorTotal, setValorTotal] = useState<number>(0);
    const { register, handleSubmit } = useForm();

    useEffect(() => {
        setOpen(props.open);
        window.setTimeout(() => {
            inputRef.current?.select();
            inputRef.current?.focus();
        }, 300);
    }, [props.open]);


    //Somar valor das entradas:
    useEffect(() => {
        var valor = 0;
        newItems.map((x) => { valor += Math.abs(x.valor); });
        setValorTotal (valor);
    }, [newItems]);


    const addItem = async (data: any) => {
        // console.log("data", data);
        let items = [...newItems];

        items.push({
            ...props.item,
            viewId: String(Math.floor(Math.random() * 1000) + 1),
            memo: String(data.memo),
            valor: Number(data.valor)
        });

        setNewItems(items);
        window.setTimeout(() => { inputRef.current?.focus(); inputRef.current?.select() }, 100);
    };


    const removeItem = (id: number) => {
        let items = newItems.filter((x) => (x.id !== id));
        setNewItems(items);
    }

    //Valor das entradas bate com o lcto original?
    const valorBate = () => {
        return valorTotal.toFixed(2) === Math.abs(props.item.valor).toFixed(2);
    }

    const salvar = () => {
        props.onChange(newItems, 'dividir');
    }

    return (
        <>
            <Dialog open={open}
                onClose={() => props.handleOpen(false)}
                PaperComponent={(props) => DialogPaperComponent(props, "dialog-title")}
                aria-labelledby="dialog-title"
            >
                <DialogTitle id="dialog-title" style={{ cursor: 'move' }} >Dividir em...</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        <Grid container>
                            <form onSubmit={handleSubmit(addItem)}>
                            <Grid container margin={1}>
                                <Grid item xs={5}>
                                    <TextField
                                        variant="filled"
                                        color="primary"
                                        label="Descrição"
                                        inputRef={inputRef}
                                        {...register("memo")}
                                        defaultValue={props.item.memo}
                                    />
                                </Grid>
                                <Grid item xs={5}>
                                    <TextField
                                        variant="filled"
                                        color="primary"
                                        label="Valor"
                                        {...register("valor")}
                                    />
                                </Grid>
                                <Grid item xs={2}>
                                    <Button variant="outlined" type="submit" color="primary" style={{ margin: 11 }}><AddBoxTwoTone /></Button>
                                </Grid>
                            </Grid>
                            </form>
                            <Grid container margin={1}>
                                {newItems.map((x) => (<Chip style={{marginRight: 10 }} key={x.newId} variant='outlined' color={valorBate() ? 'success': 'info'} label={x.memo + " " + x.valor} onDelete={() => removeItem(x.id)} />))}
                            </Grid>
                        </Grid>
                    </DialogContentText>
                </DialogContent>

                <DialogActions>
                            <Box sx={{ p: 1, marginRight: 2, 
                                bgcolor: valorBate() ? 'success.light' : 'warning.light',
                                 color: valorBate() ? 'success.contrastText' : 'warning.contrastText' }}>
                                Valor original: {props.item.valor}. Total entradas: {valorTotal}
                            </Box> 
                    <Button variant="outlined" onClick={() => props.handleOpen(false)} color="primary">Cancelar</Button>
                    <Button variant="contained" onClick={() => { salvar(); props.handleOpen(false) }} color="primary" disabled={!valorBate()}>Salvar</Button>
                </DialogActions>
            </Dialog>

        </>
    );
}

export default PopupDividir;