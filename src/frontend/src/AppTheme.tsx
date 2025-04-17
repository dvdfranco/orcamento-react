// import { createTheme } from '@material-ui'

import { createStyles, createTheme, Theme } from "@mui/material";
import { blue } from "@mui/material/colors";

const AppTheme = createTheme(
    {
        palette: {
          primary: {
            main: '#42a5f5',
          },
          secondary: {
            main: '#dce775',
          }
        }
    }
);

// export const useStyles2 = makeStyles((theme: Theme) => 
//   createStyles({
//     teste: {
//         color: 'blue'
//     }
//   })
// );

export default AppTheme;