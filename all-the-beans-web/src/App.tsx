import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import Home from './pages/Home'
import BeanList from './pages/BeanList'
import BeanDetail from './pages/BeanDetail'

function App() {
  return (
    <BrowserRouter>
      <nav className="navbar navbar-light bg-light mb-4">
        <div className="container">
          <Link className="navbar-brand" to="/">All The Beans</Link>
          <div>
            <Link className="btn btn-outline-primary me-2" to="/">Bean of the Day</Link>
            <Link className="btn btn-outline-primary" to="/beans">All Beans</Link>
          </div>
        </div>
      </nav>
      <div className="container">
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
