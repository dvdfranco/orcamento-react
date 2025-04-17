import { MainLayout } from './MainLayout';
import Principal from './pages/Principal';
import Memo from './pages/Memo';
import { Route, Routes } from 'react-router-dom';

const AppRoutes : React.FC = () => (
    <Routes>
        <Route element={<MainLayout />}>
            <Route path="/" element={<Principal/>} />
            <Route path="/memos" element={<Memo />} />
        </Route>
    </Routes>
)

export default AppRoutes;