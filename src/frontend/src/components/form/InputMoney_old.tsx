import { InputNumber, InputNumberProps } from 'primereact/inputnumber';
import React from 'react';
import { Controller } from 'react-hook-form';

interface Props extends InputNumberProps {
    label?: string;
    name: string;
    // onChange?:any;
    control?: any;
    className?: string;
    // onBlur?: any;
    singleLine?: boolean;
}

const Input: React.FC<Props> = ({label, name, control, singleLine, mode, onBlur, currency, className, ...restProps}) => {
    return (
        control !== undefined ?
            <Controller
                // name={props.name}
                name={name}
                control={control}
                {...restProps}
                render={({field}) => (
                    <>
                        <div className={singleLine ? "flex align-items-left" : "field"}>
                            <label style={singleLine ? {width:100} : undefined } className={singleLine ? "mt-2" : ""}>{label}:</label>
                        
                            <InputNumber
                                maxFractionDigits={2}
                                minFractionDigits={2}
                                onBlur={onBlur}
                                mode={mode ?? "decimal"}
                                currency={currency??undefined}
                                className={`${className??""} ${singleLine ? '' : 'w-full'}`}
                                style={singleLine ? {maxWidth: 120} : undefined}

                                //obrigatorio por causa do bug inputnumber só exibe NaN:
                                onValueChange={(e) => field.onChange(e)} 
                                value={field.value}
                            />
                        </div>
                    </>
                )}
            />
        :
            <div className={singleLine ? "flex align-items-left" : "field"}>
                {label ? <label style={singleLine ? {width:100} : undefined } className={singleLine ? "mt-2" : ""}>{label}:</label> : null}

                <InputNumber
                    maxFractionDigits={2}
                    minFractionDigits={2}
                    onBlur={onBlur}
                    mode={mode ?? "decimal"}
                    currency={currency??undefined}
                    className={`${className??""} ${singleLine ? '' : 'w-full'}`}
                    style={singleLine ? {maxWidth: 120} : undefined}
                    {...restProps}
                    //obrigatorio por causa do bug inputnumber só exibe NaN:
                    // onValueChange={(e) => field.onChange(e)} 
                    // value={field.value}
                />
            </div>
    );
}

export default Input;