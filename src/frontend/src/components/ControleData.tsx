import { Button } from 'primereact/button';
import React, { useState } from 'react'
import moment from 'moment';

interface Props {
    onChange:(data:any) => String
}

const ControleData:React.FC<Props> = (props) => {
    const [data, setData] = useState<String>(moment().format('MMMM [de] YYYY'));

    return (
        <div className="flex flex-row justify-content-center">
            <Button onClick={()=> { setData(props.onChange(-1)); }} style={{margin: 10, backgroundColor: 'var(--primary-300)'}}><span className='pi pi-angle-double-left'/></Button>
            <div className="mt-4">
                {data.substring(0,1).toUpperCase()}{data.substring(1)}
            </div>
            <Button onClick={()=> { setData(props.onChange(1)); }} style={{margin: 10, backgroundColor: 'var(--primary-300)'}}><span className='pi pi-angle-double-right'/></Button>

        </div>
    );
}

export default ControleData;