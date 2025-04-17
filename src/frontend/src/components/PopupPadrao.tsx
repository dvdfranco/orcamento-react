import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Grid, Icon, Paper, PaperProps, TextField, ToggleButton, ToggleButtonGroup, Typography } from '@mui/material';
import React, { useEffect, useRef, useState } from 'react'
import { FormatAlignLeftTwoTone, FormatAlignCenterTwoTone, FormatAlignRightTwoTone, ArrowRightTwoTone } from '@mui/icons-material';
import Draggable from 'react-draggable';
import { DialogPaperComponent, IconeCategoria } from '../utils/Utils';
import { Lancamento } from '../models/Models';
import { useForm } from 'react-hook-form';

interface Props {
    item: Lancamento,
    onChange: (data: any) => void,
    handleOpen: (data: boolean) => void,
    open: boolean
}


const PopupPadrao: React.FC<Props> = (props) => {
    // const [data, setData] = useState<String>(moment().format('MMMM [de] YYYY'));
    const [tipoPadrao, setTipoPadrao] = useState<String>("contains");
    const [open, setOpen] = React.useState(false);
    const inputRef = useRef<HTMLInputElement | null>(null);
    const { register, getValues, setValue } = useForm({});

    useEffect(() => {
        setOpen(props.open);
        window.setTimeout(() => {
            inputRef.current?.select();
            inputRef.current?.focus();
        }, 300);
    }, [props.open]);

    useEffect(() => {
        setValue('tipo', 'contains');
        setTipoPadrao('contains');
    }, []);


    const onSubmit = async () => {
        let item = { ...getValues(), idCategoria: props.item.categoria.id };
        props.onChange(item);
        props.handleOpen(false);
    }

    return (
        <>
            <Dialog open={open}
                onClose={() => props.handleOpen(false)}
                PaperComponent={(props) => DialogPaperComponent(props, "dialog-title")}
                aria-labelledby="dialog-title"
            >
                <DialogTitle id="dialog-title" style={{ cursor: 'move' }} >Definir nome padrão</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        <Grid container>
                            <Grid container margin={1}>
                                <Grid item>
                                    <ToggleButtonGroup value={tipoPadrao} exclusive onChange={(event, valor) => { setValue('tipo', valor); setTipoPadrao(valor); }} aria-label="Tipo de conteúdo">
                                        <ToggleButton value="starts" key="starts" aria-label="left aligned" title="Quando Inicia com..."><FormatAlignLeftTwoTone /></ToggleButton>
                                        <ToggleButton value="contains" key="contains" aria-label="centered" title="Quando contém..."><FormatAlignCenterTwoTone /></ToggleButton>
                                        <ToggleButton value="ends" key="ends" aria-label="right aligned" title="Qunado termina com..."><FormatAlignRightTwoTone /></ToggleButton>
                                    </ToggleButtonGroup>
                                </Grid>
                            </Grid>
                            <Grid container margin={1}>
                                <Grid item style={{ verticalAlign: 'middle' }}>
                                    <TextField
                                        variant="filled"
                                        color="primary"
                                        {...register("from")}
                                        label={tipoPadrao === 'contains' ? 'Se contém...' : tipoPadrao === 'starts' ? 'Se inicia com...' : 'Se termina com...'}
                                        inputRef={inputRef}
                                        defaultValue={props.item.bankMemo}
                                    />

                                    <Icon style={{ marginTop: 15 }}><ArrowRightTwoTone color="primary" /></Icon>

                                    <TextField
                                        variant="filled"
                                        color="primary"
                                        {...register("to")}
                                        label="Salvar como..."
                                        defaultValue={props.item.memo}
                                    />

                                    {IconeCategoria(props.item.categoria.icone)}
                                </Grid>
                            </Grid>
                        </Grid>
                    </DialogContentText>
                </DialogContent>

                <DialogActions>
                    <Button variant="outlined" onClick={() => props.handleOpen(false)} color="primary">Cancelar</Button>
                    <Button variant="contained" onClick={onSubmit} color="primary">Salvar</Button>
                </DialogActions>
            </Dialog>

        </>
    );
}

export default PopupPadrao;