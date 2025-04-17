// @flow
import { ThemeProvider } from "@mui/material";
import React from "react";
import "./App.css";
import AppTheme from "./AppTheme";
import Principal from "./pages/Principal";
import "primereact/resources/primereact.min.css";
import "primereact/resources/themes/lara-light-indigo/theme.css";     
import 'primeicons/primeicons.css';
import { TabMenu } from 'primereact/tabmenu';
import { BrowserRouter } from 'react-router-dom';
import AppRoutes from './Routes';

const App: React.FC = () => {

  return (
    <div className="App">
      <ThemeProvider theme={AppTheme}>
        <BrowserRouter>
          <AppRoutes />
        </BrowserRouter>
        {/* <header className="App-header">
          <p>
            <TabMenu model={items} />

          </p>
        </header>

        <Principal /> */}
      </ThemeProvider>
    </div>
  );
};

export default App;
