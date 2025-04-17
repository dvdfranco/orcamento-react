import { InputNumberProps } from 'primereact/inputnumber';
import { InputText } from 'primereact/inputtext';
import React, { useState } from 'react';
import { Controller } from 'react-hook-form';

interface Props extends Omit<InputNumberProps, 'value'> {
    label?: string;
    name: string;
    // onChange?:any;
    control?: any;
    className?: string;
    // onBlur?: any;
    singleLine?: boolean;
    value? : number | null;
}

const Input: React.FC<Props> = ({label, name, control, singleLine, onBlur, className, ...restProps}) => {
    const [inputValue, setInputValue] = useState(restProps.value ?? '');
    return (
        control !== undefined ?
            <Controller
                // name={props.name}
                name={name}
                control={control}
                render={({field}) => (
                        <div className={singleLine ? "flex align-items-left" : "field"}>
                            {label ? <label style={singleLine ? {width:100} : undefined } className={singleLine ? "mt-2" : ""}>{label}:</label> : null}

                            <InputText
                                onBlur={onBlur}
                                className={`${className??""} ${singleLine ? '' : 'w-full'}`}
                                style={singleLine ? {maxWidth: 120} : undefined}
                                value={field.value?.toFixed(2) || ''}
                                {...restProps}
                                onChange={(e) => {
                                    //Transformar 12345 em 123.45
                                    let valor = Number(e.target.value?.replace(/\D/g, "")) / 100;
                                    if (e.target.value?.indexOf("-") >= 0) valor = valor * -1;
                                    //TODO: se -0.00, const val = "-" + valor.toFixed(2) (se o user preencheu - antes de digitar o resto do valor)
                                    field.value = (valor.toFixed(2));
                                    field.onChange(valor);
                                }}
                            />
                        </div>
                )}
            />
        :
            <div className={singleLine ? "flex align-items-left" : "field"}>
                {label ? <label style={singleLine ? {width:100} : undefined } className={singleLine ? "mt-2" : ""}>{label}:</label> : null}

                <InputText
                    onBlur={onBlur}
                    className={`${className??""} ${singleLine ? '' : 'w-full'}`}
                    style={singleLine ? {maxWidth: 120} : undefined}
                    {...restProps}
                    value={Number(inputValue)?.toFixed(2)}
                    onChange={(e) => {
                        //Transformar 12345 em 123.45
                        let valor = Number(e.target.value?.replace(/\D/g, "")) / 100;
                        if (e.target.value?.indexOf("-") >= 0) valor = valor * -1;
                        setInputValue(valor.toFixed(2));
                    }}
                />
            </div>
    );
}

export default Input;