import React, { useState, useEffect } from 'react';
import { Container, AppBar, Toolbar, Typography, Box, Paper, CircularProgress, Button } from '@mui/material';
import axios from 'axios';
import LoginForm from './components/LoginForm';
import ConnectionForm, { ConnectionInfo } from './components/ConnectionForm';
import ObjectSelector from './components/ObjectSelector';
import DataMapper, { Mapping } from './components/DataMapper';
import ChartRenderer from './components/ChartRenderer';

// Add enum for data object types
export enum DataObjectType {
  View = 'view',
  Procedure = 'procedure',
  Function = 'function',
}

function App() {
  const [token, setToken] = useState<string | null>(null);
  const [loginError, setLoginError] = useState<string | null>(null);
  const [connection, setConnection] = useState<ConnectionInfo | null>(null);
  const [objectType, setObjectType] = useState<DataObjectType>(DataObjectType.View);
  const [objects, setObjects] = useState<string[]>([]);
  const [selectedObject, setSelectedObject] = useState<string | null>(null);
  const [objectError, setObjectError] = useState<string | null>(null);
  const [dataColumns, setDataColumns] = useState<string[]>([]);
  const [dataRows, setDataRows] = useState<any[]>([]);
  const [dataLoading, setDataLoading] = useState(false);
  const [dataError, setDataError] = useState<string | null>(null);
  const [mapping, setMapping] = useState<Mapping | null>(null);
  const [chartType, setChartType] = useState<'line' | 'bar' | 'radar'>('line');

  const handleLogout = () => {
    setToken(null);
    setLoginError(null);
    setConnection(null);
    setObjectType(DataObjectType.View);
    setObjects([]);
    setSelectedObject(null);
    setObjectError(null);
    setDataColumns([]);
    setDataRows([]);
    setDataLoading(false);
    setDataError(null);
    setMapping(null);
    setChartType('line');
  };

  useEffect(() => {
    // Set up axios interceptor for Authorization header and 401 handling
    const interceptor = axios.interceptors.request.use(config => {
      if (token) {
        config.headers = config.headers || {};
        config.headers['Authorization'] = `Bearer ${token}`;
      }
      return config;
    });
    const responseInterceptor = axios.interceptors.response.use(
      response => response,
      error => {
        if (error.response && error.response.status === 401) {
          handleLogout();
        }
        return Promise.reject(error);
      }
    );
    return () => {
      axios.interceptors.request.eject(interceptor);
      axios.interceptors.response.eject(responseInterceptor);
    };
  }, [token]);

  const fetchObjects = async (info: ConnectionInfo, type: DataObjectType) => {
    setObjects([]);
    setObjectError(null);
    try {
      // Clean host value to prevent double escaping
      const cleanInfo = {
        ...info,
        host: info.host.replace(/\\/g, '\\'),
        type,
      };
      console.log('Sending request with host:', cleanInfo.host);
      const res = await axios.post('https://localhost:7185/api/Data/objects', cleanInfo);
      setObjects(res.data.data || []);
    } catch (err: any) {
      setObjectError(err.response?.data?.message || 'Failed to fetch objects');
    }
  };

  const handleLogin = async (username: string, password: string) => {
    try {
      const response = await axios.post('https://localhost:7185/api/Auth/token', { username, password });
      setToken(response.data.token);
      setLoginError(null);
    } catch (err: any) {
      setLoginError(err.response?.data?.message || 'Login failed');
    }
  };

  const handleConnect = async (info: ConnectionInfo) => {
    setConnection(info);
    setSelectedObject(null);
    setDataColumns([]);
    setDataRows([]);
    setMapping(null);
    fetchObjects(info, objectType);
  };

  const handleTypeChange = (type: DataObjectType) => {
    setObjectType(type);
    if (connection) {
      setSelectedObject(null);
      setDataColumns([]);
      setDataRows([]);
      setMapping(null);
      fetchObjects(connection, type);
    }
  };

  const handleObjectSelect = async (objectName: string) => {
    setSelectedObject(objectName);
    setDataColumns([]);
    setDataRows([]);
    setMapping(null);
    if (!connection) return;
    setDataLoading(true);
    setDataError(null);
    try {
      // Clean host value to prevent double escaping
      const cleanConnection = {
        ...connection,
        host: connection.host.replace(/\\/g, '\\'),
        objectName,
        objectType: objectType === DataObjectType.Procedure ? 'SP' : objectType.charAt(0).toUpperCase() + objectType.slice(1),
      };
      console.log('Sending execute request with host:', cleanConnection.host);
      const res = await axios.post('https://localhost:7185/api/Data/execute', cleanConnection);
      setDataColumns(res.data.data.columns || []);
      setDataRows(res.data.data.rows || []);
    } catch (err: any) {
      setDataError(err.response?.data?.message || 'Failed to fetch data');
    } finally {
      setDataLoading(false);
    }
  };

  const handleMappingChange = (m: Mapping) => {
    setMapping(m);
  };

  // TODO: Add ChartRenderer integration

  return (
    <Box sx={{ bgcolor: '#f5f6fa', minHeight: '100vh' }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Dynamic Chart Visualizer
          </Typography>
          {token && (
            <Button color="inherit" onClick={handleLogout} sx={{ ml: 2 }}>
              Logout
            </Button>
          )}
        </Toolbar>
      </AppBar>
      <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
        {!token ? (
          <LoginForm onLogin={handleLogin} error={loginError || undefined} />
        ) : (
          <Paper elevation={3} sx={{ p: 3 }}>
            {!connection ? (
              <ConnectionForm onConnect={handleConnect} />
            ) : (
              <>
                <ObjectSelector
                  objects={objects}
                  type={objectType}
                  selectedObject={selectedObject}
                  onTypeChange={handleTypeChange}
                  onSelect={handleObjectSelect}
                />
                {objectError && <Typography color="error" mt={2}>{objectError}</Typography>}
                {dataLoading && <Box display="flex" justifyContent="center" my={2}><CircularProgress /></Box>}
                {dataError && <Typography color="error" mt={2}>{dataError}</Typography>}
                {dataColumns.length > 0 && (
                  <DataMapper columns={dataColumns} onMappingChange={handleMappingChange} />
                )}
                {mapping && (
                  <ChartRenderer
                    dataRows={dataRows}
                    mapping={mapping}
                    chartType={chartType}
                    onChartTypeChange={setChartType}
                  />
                )}
                {/* Step 4: Chart Renderer */}
                {/* <ChartRenderer /> */}
              </>
            )}
          </Paper>
        )}
      </Container>
    </Box>
  );
}

export default App;
