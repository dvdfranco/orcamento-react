import React from 'react'
import {
    RestaurantTwoTone, DirectionsCarTwoTone, DescriptionTwoTone, BrushTwoTone, MedicationTwoTone, HouseTwoTone,
    CakeTwoTone, LocalGroceryStoreTwoTone, PersonTwoTone, PetsTwoTone, SavingsTwoTone, RedeemTwoTone,
    AttachMoneyTwoTone, CheckroomTwoTone, LocalAtmTwoTone, MedicationLiquidTwoTone, QuestionMarkTwoTone, ShoppingBasketTwoTone, FilterAltTwoTone, HelpOutlineTwoTone
} from '@mui/icons-material';
import { Paper, PaperProps } from '@mui/material';
import Draggable from 'react-draggable';


//Componente pra tornar Dialog arrastavel
export function DialogPaperComponent(props: PaperProps, handleId: string) {
    return (
        <Draggable
            handle={"#" + handleId}
            cancel={'[class*="MuiDialogContent-root"]'}
        >
            <Paper {...props} />
        </Draggable>
    );
}

export function IconeCategoria(iconCode: string) {
    switch (iconCode) {
        case 'fa-apple-alt': return (<RestaurantTwoTone />);
        case 'fa-car': return (<DirectionsCarTwoTone />);
        case 'fa-shopping-basket': return (<ShoppingBasketTwoTone />);
        case 'fa-file-invoice-dollar': return (<DescriptionTwoTone />);
        case 'fa-paint-brush': return (<BrushTwoTone />);
        case 'fa-dollar-sign': return (<AttachMoneyTwoTone />);
        case 'fa-prescription-bottle-alt': return (<MedicationTwoTone />);
        case 'fa-house-damage': return (<HouseTwoTone/>);
        case 'fa-candy-cane': return (<CakeTwoTone />);
        case 'fa-shopping-cart': return (<LocalGroceryStoreTwoTone />);
        case 'fa-child': return (<PersonTwoTone />);
        case 'fa-home': return (<HouseTwoTone />);
        case 'fa-fish': return (<PetsTwoTone />);
        case 'fa-piggy-bank': return (<SavingsTwoTone />);
        case 'fa-gift': return (<RedeemTwoTone />);
        case 'fa-tshirt': return (<CheckroomTwoTone />);
        case 'fa-money-bill-alt': return (<LocalAtmTwoTone />);
        case 'fa-briefcase-medical': return (<MedicationLiquidTwoTone />);
        case 'fa-shopping-bag': return (<ShoppingBasketTwoTone />);
        case 'fa-question': return (<QuestionMarkTwoTone />);
        case 'FilterAltTwoTone': return (<FilterAltTwoTone />);
        case 'fa-question-circle': return (<HelpOutlineTwoTone  color='error' />);
        default: return (<QuestionMarkTwoTone />);
    }
}
/*
Alimentacao        fa-apple-alt                Restaurant
Carro              fa-car                      DirectionsCar
Compras            fa-shopping-basket          ShoppingBasket
Contas             fa-file-invoice-dollar      Description
Cosméticos         fa-paint-brush              Brush
Dízimo             fa-dollar-sign              AttachMoney
Farmácia           fa-prescription-bottle-alt  Medication
Gastos Domésticos  fa-house-damage             House
Gordices           fa-candy-cane               Cake
Mercado            fa-shopping-cart            LocalGroceryStore
Miguel             fa-child                    Person
Moradia            fa-home                     House
Pets               fa-fish                     Pets
Poupança           fa-piggy-bank               Savings
Presentes          fa-gift                     Redeem
Recebimentos       fa-dollar-sign              AttachMoney
Roupas             fa-tshirt                   Checkroom
Salário            fa-money-bill-alt           LocalAtm
Saques             fa-dollar-sign              AttachMoney
Saúde              fa-briefcase-medical        MedicationLiquid
Sem Categoria      fa-question                 QuestionMark
Shopping/Lazer     fa-shopping-bag             ShoppingBasket
Tarifas            fa-dollar-sign              AttachMoney
*/