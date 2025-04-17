import { InputNumber, InputNumberProps } from 'primereact/inputnumber';
import { InputText, InputTextProps } from 'primereact/inputtext';
import React from 'react';
import { Controller } from 'react-hook-form';

interface Props extends InputNumberProps {
    label?: string;
    name: string;
    // onChange?:any;
    control?: any;
    className?: string;
    // onBlur?: any;
}

const Input: React.FC<Props> = (props) => {
    return (
        <Controller
            // name={props.name}
            control={props.control}
            {...props}
            render={({field}) => (
                <><div>
                    <label>{props.label} {props.className}</label></div>
                    <div>
                        <InputText
                            className={props.className}
                            {...field}
                        />
                    </div>
                </>
            )}
        />
    );
}

export default Input;