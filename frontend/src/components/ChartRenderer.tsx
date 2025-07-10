/**
 * ChartRenderer component for rendering dynamic charts using Chart.js.
 */
import React from 'react';
import { Box, Typography, ToggleButton, ToggleButtonGroup } from '@mui/material';
import { Line, Bar, Radar } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  RadialLinearScale,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Mapping } from './DataMapper';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  RadialLinearScale,
  Title,
  Tooltip,
  Legend
);

export interface ChartRendererProps {
  dataRows: any[];
  mapping: Mapping;
  chartType: 'line' | 'bar' | 'radar';
  onChartTypeChange: (type: 'line' | 'bar' | 'radar') => void;
}

const ChartRenderer: React.FC<ChartRendererProps> = ({ dataRows, mapping, chartType, onChartTypeChange }) => {
  if (!mapping.x || !mapping.y) return null;

  const labels = dataRows.map(row => row[mapping.x]);
  const data = dataRows.map(row => Number(row[mapping.y]));

  const chartData = {
    labels,
    datasets: [
      {
        label: mapping.y,
        data,
        backgroundColor: 'rgba(54, 162, 235, 0.5)',
        borderColor: 'rgba(54, 162, 235, 1)',
        borderWidth: 2,
        fill: chartType !== 'bar',
      },
    ],
  };

  return (
    <Box mt={4}>
      <Typography variant="h6" gutterBottom>
        Chart
      </Typography>
      <ToggleButtonGroup
        value={chartType}
        exclusive
        onChange={(_, val) => val && onChartTypeChange(val)}
        sx={{ mb: 2 }}
      >
        <ToggleButton value="line">Line</ToggleButton>
        <ToggleButton value="bar">Bar</ToggleButton>
        <ToggleButton value="radar">Radar</ToggleButton>
      </ToggleButtonGroup>
      {chartType === 'line' && <Line data={chartData} />}
      {chartType === 'bar' && <Bar data={chartData} />}
      {chartType === 'radar' && <Radar data={chartData} />}
    </Box>
  );
};

export default ChartRenderer; 