import { Chart } from 'primereact/chart';
import React, { useContext, useEffect, useState } from 'react'
import { Lancamento } from '../models/Models';
import moment from 'moment';
import LctoApi from '../services/api/LctoApi';
import { LctoContext } from '../context/LctoContext';

interface Props {
    dataRef: moment.Moment;
}

const Graficos:React.FC<Props> = (props) => {
    const [chartData, setChartData] = useState({});
    const [lineChartCategorias, setLineChartCategorias] = useState<any[]>([]);

    const lctoContext = useContext(LctoContext);

    interface ILineData {
        nomeCategoria: string;
        labels: string[];
        valores: number[];
    }

    interface IEstatisticasData {
        pieData: Lancamento[];
        lineData: ILineData[]
    }

    const hoverColor = [
        "rgba(178, 69, 92, 0.5)",
        "rgba(38, 114, 164, 0.5)",
        "rgba(52, 134, 134, 0.5)",
        "rgba(107, 71, 178, 0.5)",
        "rgba(178, 144, 60, 0.5)",
        "rgba(178, 111, 45, 0.5)",
        "rgba(178, 178, 71, 0.5)",
        "rgba(178, 71, 107, 0.5)",
        "rgba(142, 178, 107, 0.5)",
        "rgba(142, 71, 0, 0.5)",
        "rgba(125, 178, 71, 0.5)",
        "rgba(98, 71, 178, 0.5)",
        "rgba(151, 71, 178, 0.5)",
        "rgba(178, 71, 125, 0.5)",
        "rgba(107, 0, 107 0.5)",
    ];

    const backgroundColor = [
        "rgba(255, 99, 132, 0.5)",
        "rgba(54, 162, 235, 0.5)",
        "rgba(75, 192, 192, 0.5)",
        "rgba(153, 102, 255, 0.5)",
        "rgba(255, 206, 86, 0.5)",
        "rgba(255, 159, 64, 0.5)",
        "rgba(255, 255, 102, 0.5)",
        "rgba(255, 102, 153, 0.5)",
        "rgba(204, 255, 153, 0.5)",
        "rgba(204, 102, 0, 0.5)",
        "rgba(179, 255, 102, 0.5)",
        "rgba(140, 102, 255, 0.5)",
        "rgba(217, 102, 255, 0.5)",
        "rgba(255, 102, 179, 0.5)",
        "rgba(153, 0, 153, 0.5)"
    ];

    const configPieChart = (lctos: Lancamento[]): void => {
        const lctoMemos = lctos.map((x) => (x.memo));
        const lctoValores = lctos.map((x) => (Math.abs(x.valor)));

        const data = {
            labels: lctoMemos,
            datasets: [
                {
                    data: lctoValores,
                    backgroundColor: backgroundColor,
                      hoverBackgroundColor: hoverColor
                }
            ]
        }
        

        setChartData(data);
    }
    const pieChartOptions = {
            plugins: {
                legend: {
                    labels: {
                        usePointStyle: true
                    }
                }
            }
        };

    
    const lineChartOptions = {
        responsive: true,
        plugins: {
            legend: { position: 'top' },
            title: { display: true, text: 'Evolução de gastos' }
        }
    };

    const lineChart = (categoria: ILineData): JSX.Element => {
        const data = {
            labels: categoria.labels,
            datasets: [{ label: categoria.nomeCategoria, data: categoria.valores }]
        };

        return (
            <div className="card flex justify-content-center" key={categoria.nomeCategoria}>
                <div className="flex col-9">
                    <Chart type="line" data={data} options={lineChartOptions} width='300' height='150' className='w-full' />
                </div>
            </div>
        )
    }

    useEffect(() => {
        if (!props.dataRef || !lctoContext.listaLcto)
            return;

        LctoApi.getEstatisticas(props.dataRef.format("YYYYMM")).then((data: IEstatisticasData) => {
            configPieChart(data.pieData);
            setLineChartCategorias(data.lineData);
        });
    }, [lctoContext.listaLcto]);

    return (
        <>
            <div className="card flex justify-content-center">
                <div className=" flex col-8">
                    <Chart type="pie" data={chartData} options={pieChartOptions} className="w-full" />
                </div>
            </div>
            {lineChartCategorias.map(x => lineChart(x))}
        </>
    );
}

export default Graficos;