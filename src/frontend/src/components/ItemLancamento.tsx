import React, { useContext, useEffect, useRef, useState } from "react";
import { Lancamento } from "../models/Models";
import styles from './ListaLancamento.module.css';
import { AutoComplete } from "primereact/autocomplete";
import { CustomMemoContext } from "../context/CustomMemoContext";

interface Props {
  item: Lancamento;
  handleChange: (data: any, tipoAlteracao: string, itemPrevistoOriginal?: string) => void;
}

interface Option {
  label: string;
  value?: Lancamento;
}

const ItemLancamento: React.FC<Props> = (props) => {
  const [memos, setMemos] = useState<Lancamento[]>([]);
  const [memo, setMemo] = useState<string>("");
  const [memoOriginal, setMemoOriginal] = useState(""); // Guardar valor do campo antes de alterar
  const [edit, setEdit] = useState<boolean>(false);
  const inputRef = useRef<HTMLInputElement | null>(null);
  const memoContext = useContext(CustomMemoContext);
  const [ memoOriginalLctoPrevisto, setMemoOriginalLctoPrevisto ] = useState(""); // Indica se esse item eh previsto quando entrou na lista.. util para saber caso for editar o nome e remover da lista de previstos

  useEffect(() => {
    setMemo(props.item.memo);
    setMemoOriginal(props.item.memo);
    
    if (props.item.isPrevisto)
      setMemoOriginalLctoPrevisto(props.item.memo);
  }, [props.item]);

  useEffect(() => {
    if (inputRef.current !== null) {
      inputRef.current?.focus();
      window.setTimeout(() => { inputRef.current?.select(); }, 50);
    }
  }, [edit]);


  //Ao escolher uma opcao do autocomplete:
  const onChangeOption = (data: any) => {
    //Se usuario digitou um valor fora da lista, formatar o valor conforme esperado:
    //Nesse caso, IdCategoria = o mesmo do prop, sem alteracao
    //TODO: incluir isso na lista de Custom Memos!
    if (typeof data === 'string')
        data = { value: { memo: data, idCategoria: props.item.idCategoria, categoria: props.item.categoria, idConta: props.item.idConta } };
    
    setMemos([]);

    if (data?.value !== null)
    {
        let lcto: Lancamento = props.item;
        lcto.memo = data?.value?.memo;
        lcto.idCategoria = data?.value?.idCategoria;
        lcto.categoria = data?.value?.categoria;
        lcto.isPrevisto = data?.value?.isPrevisto;

        // console.log("new memo", lcto.memo);
        // console.log("new idCategoria", lcto.idCategoria);
        // console.log("new categoria", lcto.categoria.id);
        // console.log("new categoria", lcto.categoria.descricao);

        setEdit(false);
        setMemo(lcto.memo);
        props.handleChange(lcto, 'memo', memoOriginalLctoPrevisto);
    }
  };

  
  //Saiu o foco? atualiza
  // const onBlurText = (event: React.FocusEvent<HTMLInputElement>) => {
  //   onChangeOption(event, { value: { ...props.item, memo: event.target.value }});
  // }
  
  //Acoes Esc/Enter
  const onKeyUpText = (event: React.KeyboardEvent<HTMLInputElement>) => {
      let newMemo = event.currentTarget?.value;
      if (newMemo === '') newMemo = memoOriginal; // Se apagou e deu Enter/esc, deve voltar pro valor original

      if (event.key === "Escape") {
        event.preventDefault();
        setEdit(false);
        setMemo(memoOriginal);
      }
      else if (event.key === "Enter" || event.key === "NumPadEnter")
      {
        onChangeOption({ value: { ...props.item, memo: newMemo, isPrevisto: false  }});
        event.preventDefault();
      }
  }

  const searchMemos = (event:any) => {
    setMemos(
      memoContext.customMemos.filter((memo:Lancamento) => (memo.memo.toLowerCase().indexOf(event.query.toLowerCase()) >= 0))
    );
  }

  return (
    <>
      {edit ? (
        <AutoComplete
          value={memo}
          suggestions={memos}// .map((x) => (x.memo))}
          completeMethod={searchMemos}
          onChange={(e) => { setMemo(e.value)}}
          onKeyUp={onKeyUpText}
          inputRef={inputRef}
          onSelect={(e) => onChangeOption({ value: e.value }) }
          field="memo"
         />
      ) : (
        <span
          onDoubleClick={(e: any) => {
            setEdit(true);
          }}
          className={props.item.transid === 0 ? styles.naopago : styles.pago}
        >
          {memo}
        </span>
      )}
    </>
  );
};

export default ItemLancamento;
