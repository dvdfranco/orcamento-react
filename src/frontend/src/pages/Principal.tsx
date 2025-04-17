import { Button, Grid, Menu, MenuItem } from "@mui/material";
import moment from "moment";
import 'moment/locale/pt-br';
import React, { useContext, useEffect, useState } from "react";
import ControleData from "../components/ControleData";
import ListaLancamento from "../components/ListaLancamento";
import { Categoria, Lancamento } from "../models/Models";
import LctoApi from "../services/api/LctoApi";
import LctoPrevistoApi from "../services/api/LctoPrevistoApi";
import LancamentoServices from "../services/LancamentoServices";
import MemoPadraoServices from "../services/MemoPadraoServices";
import { IconeCategoria } from "../utils/Utils";
import ResumoSaldo from "../components/ResumoSaldo";
import Graficos from "../components/Graficos";
import { CategoriaContext } from "../context/CategoriaContext";
import { LctoContext } from "../context/LctoContext";

const Principal: React.FC = () => {
  const [dataRef, setDataRef] = useState<any>(moment().date(1));
  const [filtroCategoria, setFiltroCategoria] = useState<Categoria | null>(null);
  const [saldoCalc, setSaldoCalc] = useState<any>({
    saldoHoje: 0,
    dataSaldoHoje: 0,
    saldoAnterior: 0,
    soma: 0,
    saldoCalculado: 0,
    saldoPrevisto: 0
  });

  const categoriaContext = useContext(CategoriaContext);
  const lctoContext = useContext(LctoContext)

  //Props pro filtro categoria:
  const [anchorMenuFiltroCat, setAnchorMenuFiltroCat] = useState<null | HTMLElement>(null);
  const openMenuFiltroCat = Boolean(anchorMenuFiltroCat);
  const handleClickMenuFiltroCat = (event: React.MouseEvent<HTMLButtonElement>) => { setAnchorMenuFiltroCat(event.currentTarget); };

  useEffect(() => {
    lctoContext.updateListaLctoFromData(dataRef);

    calcSaldos();
  }, [dataRef]);

  // ativar se precisar atualizar o saldo ao alterar um lancamento (ex, dividir?)
  // useEffect(() => {
  //   calcSaldos();
  // }, [listaLcto]);

  const calcSaldos = () => {
    LctoApi.getCalcSaldos(dataRef.format("YYYYMM")).then((data) => {
      setSaldoCalc(data);
    })
  };

  const alterarMes = (add: Number) => {
    let data = moment().date(1); //hoje

    if (dataRef !== "")
      data  = moment(dataRef); //moment(dataRef.substring(0,4) + "-" + dataRef.substring(4) + "-01");

    data.add(Number(add), 'M');
    
    setDataRef(data);

    return data.format("MMMM [de] YYYY"); //retorna string com o mes atual 
  };


  //Atualizar lancamento!
  const handleLctoChange = async (lcto: Lancamento | Lancamento[], tipoAlteracao: string, itemPrevistoOriginal?: string) => {
    // console.log("update!", lcto, tipoAlteracao);

    let lista:Lancamento[] = [ ...lctoContext.listaLcto ];

    switch (tipoAlteracao) //.substring(0, 4) === 'data')
    {
      case 'memo':
      case 'cat':
      case 'autocomplete':
        lista = await LancamentoServices.update(lcto as Lancamento, lista, itemPrevistoOriginal);
        break;
      case 'delete':
        lista = await LancamentoServices.delete(lcto as Lancamento, lista);
        break;
      case 'data+1':
      case 'data-1':
        lista = await LancamentoServices.updateDataRef(lcto as Lancamento, (tipoAlteracao.indexOf('+') >= 0 ? 1 : -1), lista);
        break;
      case 'dividir':
          lista = await LancamentoServices.dividir(lcto as Lancamento[], lista);
          break;

        default:
        break;
    }

    lctoContext.updateListaLcto(lista);
  }

  const handlePadraoChange = async (data: any) => {
    await MemoPadraoServices.add(data);
  }

      
  const handleClickFiltroCat = (cat: Categoria | null) => {
    setAnchorMenuFiltroCat(null); //Fechar menu
    setFiltroCategoria(cat);
  }

  return (
    <>
    <ControleData onChange={alterarMes} />

    <div className="flex">
        <Grid container className="flex-row">
          <Grid item xs={4}>        
            <ListaLancamento
              somentePrevistos={true}
              handleChange={handleLctoChange}
              handlePadraoChange={handlePadraoChange}
              />
          </Grid>
          <Grid item xs={4}>
            <ListaLancamento
              somentePrevistos={false}
              filtroCategoria={filtroCategoria}
              handleChange={handleLctoChange}
              handlePadraoChange={handlePadraoChange}
              />
          </Grid>
          <Grid item xs={4} className="flex flex-column align-content-left">
            
            <Button
                id="btFiltroCat"
                aria-controls={openMenuFiltroCat ? 'basic-menu' : undefined}
                aria-haspopup="true"
                aria-expanded={openMenuFiltroCat ? 'true' : undefined}
                onClick={handleClickMenuFiltroCat}
                variant="contained"
                color="secondary"
                title="Filtrar Categorias"
                className="flex w-3rem mb-3"
            >
                {IconeCategoria(filtroCategoria?.icone ?? "FilterAltTwoTone")}
            </Button>
            <Menu
                id="catMenu"
                anchorEl={anchorMenuFiltroCat}
                open={openMenuFiltroCat}
                onClose={() => handleClickFiltroCat(null)}
                MenuListProps={{ 'aria-labelledby': 'btCat'}}
            >
                <MenuItem key={0} onClick={() => handleClickFiltroCat(null)} ><span style={{marginRight: 40}}>{IconeCategoria(']FilterAltTwoTone')}</span> Nenhum</MenuItem>
                {categoriaContext.categorias
                    .map((cat: Categoria) => (
                    <MenuItem key={cat.id} onClick={() => handleClickFiltroCat(cat)} ><span style={{marginRight: 40}}>{IconeCategoria(cat.icone)}</span> {cat.descricao}</MenuItem>
                ))}
            </Menu>

            <ResumoSaldo Saldos={saldoCalc} onUpdate={calcSaldos} dataRef={dataRef} />

            <Graficos dataRef={dataRef} />
          </Grid>
        </Grid>
      </div>
    </>
  );
};

export default Principal;
