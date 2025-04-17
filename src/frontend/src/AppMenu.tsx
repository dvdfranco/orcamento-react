import React, { useEffect, useState } from "react";
import { TabMenu } from 'primereact/tabmenu';
import { Dialog } from 'primereact/dialog';
import { useNavigate, useLocation } from 'react-router-dom';
import Upload from "./components/Upload";

const AppMenu: React.FC = () => {
  const [ activeIndex, setActiveIndex ] = useState(0);
  const navigate = useNavigate();
  const location = useLocation();
  const [ dialogHeader, setDialogHeader] = useState<string | undefined>("");
  const [ dialogVisible, setDialogVisible] = useState(false);
  const [ dialogContent, setDialogContent ] = useState<any>();

  const items = [
    {label: 'Lancamentos', icon: 'pi pi-fw pi-home', url:'/'},
    {label: 'Memos', icon: 'pi pi-fw pi-cog', url: '/memos' },
    {label: 'Upload', icon: 'pi pi-fw pi-calendar', url: '/upload', data: <Upload onClose={() => setDialogVisible(false) } />},
  ];
  
  useEffect(() => {
    for(let i in items)
    {
      if (items[i].url == location.pathname) setActiveIndex(Number(i));
    }
  }, [])

  return (
      // <header className="App-header">
      <>
        <TabMenu model={items} activeIndex={activeIndex} onTabChange={(event) => { 
          if (event?.value?.data)
          {
            setDialogVisible(true);
            setDialogContent(event.value.data);
            setDialogHeader(event.value.label);
            event.originalEvent.preventDefault();
          }
          else
            navigate(String(event?.value?.url));
        }} />
        <Dialog header={dialogHeader} style={{ width: '30vw' }} onHide={()=> setDialogVisible(false)} visible={dialogVisible}>
            {dialogContent}
        </Dialog>
      </>
      // </header>
  );
};

export default AppMenu;
