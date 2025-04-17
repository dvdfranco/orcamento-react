import { Button, Grid, Menu, MenuItem } from '@mui/material';
import React, { useContext, useState } from 'react';
import { Categoria, Lancamento } from '../models/Models';
import {
    ArrowLeftTwoTone, ArrowRightTwoTone, ContentCopyTwoTone, DeleteTwoTone, LibraryAddCheckTwoTone, CheckTwoTone, BlockTwoTone
 } from '@mui/icons-material';
import PopupPadrao from './PopupPadrao';
import { IconeCategoria } from '../utils/Utils';
import PopupDividir from './PopupDividir';
import moment from 'moment';
import { CategoriaContext } from '../context/CategoriaContext';


interface Props {
    cat: Categoria;
    item: Lancamento;
    handleChange: (data: any, tipoAlteracao: string) => void;
    handlePadraoChange: (data: any) => void;
}

const ItemMenu: React.FC<Props> = (props) => {
    const [anchor, setAnchor] = useState<null | HTMLElement>(null);
    const openMenu = Boolean(anchor);
    const [ openPopupPadrao, setOpenPopupPadrao ] = useState<boolean>(false);
    const [ openPopupDividir, setOpenPopupDividir ] = useState<boolean>(false);
    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => { setAnchor(event.currentTarget); };
    const categoriaContext = useContext(CategoriaContext);

    const categoriaClick = (cat: Categoria | null) => {
        setAnchor(null); //Fechar menu

        //Atualizar com a categoria escolhida
        if (cat !== null)
        {
            let item = props.item;
            item.categoria = cat;
            item.idCategoria = cat.id;

            props.handleChange(item, 'cat');
        }
    }

    const handlePadrao = (open: boolean) => {
        setOpenPopupPadrao(open);
    }

    const handleDividir = (open: boolean) => {
        setOpenPopupDividir(open);
    }

    const handleAutocomplete = () => {
        let item = props.item;
        item.autocomplete = !item.autocomplete;
        props.handleChange(item, 'autocomplete');
    }


    return (
        <>
            <Button
                id="btCat"
                aria-controls={openMenu ? 'basic-menu' : undefined}
                aria-haspopup="true"
                aria-expanded={openMenu ? 'true' : undefined}
                onClick={handleClick}
                variant="contained"
                color="secondary"
                title={props.cat.descricao}
            >
                {IconeCategoria(props.cat.icone)}
            </Button>
            <Menu
                id="catMenu"
                anchorEl={anchor}
                open={openMenu}
                onClose={() => categoriaClick(null)}
                MenuListProps={{ 'aria-labelledby': 'btCat'}}
            >
                <MenuItem key={0}>
                    Mês
                    <>
                        <Button variant="outlined" color="primary" key={11} style={{marginLeft: 30 }} onClick={() => props.handleChange(props.item, 'data-1')} title={'Mover para ' + moment(props.item.dataRef).add(-1, 'M').format('MMMM [de] YYYY')}><ArrowLeftTwoTone color="primary" /></Button>
                        <Button variant="outlined" color="primary" key={12} style={{marginLeft: 10 }} onClick={() => props.handleChange(props.item, 'data+1')} title={'Mover para ' + moment(props.item.dataRef).add(1, 'M').format('MMMM [de] YYYY')}><ArrowRightTwoTone color="primary"  /></Button>
                    </>
                </MenuItem>
                <MenuItem key={1} onClick={() => { handlePadrao(true); setAnchor(null); } }><LibraryAddCheckTwoTone color="primary" style={{marginRight: 40}}/>Padrão</MenuItem>
                <MenuItem key={2} onClick={() => { handleDividir(true); setAnchor(null); } }><ContentCopyTwoTone color="primary" style={{marginRight: 40}} />Dividir</MenuItem>
                <MenuItem key={3} onClick={() => { handleAutocomplete(); setAnchor(null); }}>{props.item.autocomplete ? ( <CheckTwoTone color="primary" style={{marginRight: 40}} />) : (<BlockTwoTone color="primary" style={{marginRight: 40}} />) }Autocomplete: {props.item.autocomplete ? "Sim" : "Não"} </MenuItem>
                <MenuItem key={4} style={{borderColor: 'black', borderWidth: 1, borderStyle:'none none solid none'}} onClick={() => { if (window.confirm('Excluir????????')) props.handleChange(props.item, 'delete')}}><DeleteTwoTone color="primary"  style={{marginRight: 40}} />Excluir</MenuItem>

                {categoriaContext.categorias
                    // .sort((a:Categoria, b:Categoria) => (a.descricao > b.descricao) ? 1 : -1)
                    .map((cat: Categoria) => (
                    <MenuItem key={cat.id} onClick={() => categoriaClick(cat)}><span style={{marginRight: 40}}>{IconeCategoria(cat.icone)}</span> {cat.descricao}</MenuItem>
                ))}
            </Menu>

            <PopupPadrao open={openPopupPadrao} onChange={props.handlePadraoChange} handleOpen={handlePadrao} item={props.item} />
            <PopupDividir open={openPopupDividir} onChange={props.handleChange} handleOpen={handleDividir} item={props.item} />
        </>
    )
}

export default ItemMenu;

