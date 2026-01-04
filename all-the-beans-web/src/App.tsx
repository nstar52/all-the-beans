import { useState } from 'react'
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import Home from './pages/Home'
import BeanList from './pages/BeanList'
import BeanDetail from './pages/BeanDetail'
import OrderForm from './pages/OrderForm'

function App() {
  const [menuOpen, setMenuOpen] = useState(false)

  return (
    <BrowserRouter>
      <nav className="navbar navbar-expand-lg navbar-light bg-light mb-4">
        <div className="container">
          <Link className="navbar-brand" to="/">All The Beans</Link>
          <button 
            className="navbar-toggler" 
            type="button" 
            onClick={() => setMenuOpen(!menuOpen)}
            aria-label="Toggle navigation"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className={`collapse navbar-collapse ${menuOpen ? 'show' : ''}`}>
            <div className="navbar-nav ms-auto">
              <Link className="btn btn-outline-primary me-2" to="/" onClick={() => setMenuOpen(false)}>Bean of the Day</Link>
              <Link className="btn btn-outline-primary" to="/beans" onClick={() => setMenuOpen(false)}>All Beans</Link>
            </div>
          </div>
        </div>
      </nav>
      <div className="container">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/beans" element={<BeanList />} />
          <Route path="/beans/:id" element={<BeanDetail />} />
          <Route path="/order/:beanId?" element={<OrderForm />} />
        </Routes>
      </div>
    </BrowserRouter>
  )
}

export default App
