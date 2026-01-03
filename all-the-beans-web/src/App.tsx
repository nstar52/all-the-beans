import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Home from './pages/Home'
import BeanList from './pages/BeanList'
import BeanDetail from './pages/BeanDetail'

function App() {
  return (
    <BrowserRouter>
      <div className="container mt-4">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/beans" element={<BeanList />} />
          <Route path="/beans/:id" element={<BeanDetail />} />
        </Routes>
      </div>
    </BrowserRouter>
  )
}

export default App
