import React, { useEffect, useRef, useState } from 'react';
import { FileUpload } from 'primereact/fileupload'
import { ToastContext } from "./../MainLayout";
import { ProgressBar } from 'primereact/progressbar';
import { Navigate, useNavigate } from 'react-router-dom';

interface Props {
    onClose: () => void;
}

const Upload: React.FC<Props> = ({ onClose }) => {
    const toast = React.useContext(ToastContext);
    const fileRef = useRef<any>();
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    
    useEffect(() => {
        fileRef.current?.getInput().click();
    }, [fileRef])

    return (
        <div className="flex flex-column">
            <label htmlFor="file">Upload (Arquivo OFX):</label>
            <FileUpload
                name="file"
                mode="basic"
                url={`${process.env.REACT_APP_API_BASE_URL}/lancamento/upload`}
                onBeforeSend={() => setLoading(true)}
                onUpload={(res) => {
                    const response = JSON.parse(res.xhr.response);

                    if (response?.sucesso === true)
                        toast.showToast('success', 'Upload realizado!', "");
                    else
                        toast.showToast('error', 'ERRO ERRO ERRO!', response.msgerro);

                    setLoading(false);
                    onClose();
                    window.location.reload();
                }}
            />

            {loading &&
            <ProgressBar mode="indeterminate" className='mt-2'/>}
        </div>
    );
}

export default Upload;